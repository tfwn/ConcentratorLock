
#include <aes.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>


#define BPOLY 0x1b //!< Lower 8 bits of (x^8+x^4+x^3+x+1), ie. (x^4+x^3+x+1).
#define BLOCKSIZE 16 //!< Block size in number of bytes.

#define KEY_COUNT 1

#if KEY_COUNT == 1
  #define KEYBITS 128 //!< Use AES128.
#elif KEY_COUNT == 2
  #define KEYBITS 192 //!< Use AES196.
#elif KEY_COUNT == 3
  #define KEYBITS 256 //!< Use AES256.
#else
  #error Use 1, 2 or 3 keys!
#endif

#if KEYBITS == 128
  #define ROUNDS 10 //!< Number of rounds.
  #define KEYLENGTH 16 //!< Key length in number of bytes.
#elif KEYBITS == 192
  #define ROUNDS 12 //!< Number of rounds.
  #define KEYLENGTH 24 //!< // Key length in number of bytes.
#elif KEYBITS == 256
  #define ROUNDS 14 //!< Number of rounds.
  #define KEYLENGTH 32 //!< Key length in number of bytes.
#else
  #error Key must be 128, 192 or 256 bits!
#endif


#define EXPANDED_KEY_SIZE (BLOCKSIZE * (ROUNDS+1)) //!< 176, 208 or 240 bytes.

static uint8 AES_Key_Table[16] =
{
  0xd0, 0x94, 0x3f, 0x8c, 0x29, 0x76, 0x15, 0xd8,
  0x20, 0x40, 0xe3, 0x27, 0x45, 0xd8, 0x48, 0xad,
  //0xea, 0x8b, 0x2a, 0x73, 0x16, 0xe9, 0xb0, 0x49,
  //0x45, 0xb3, 0x39, 0x28, 0x0a, 0xc3, 0x28, 0x3c,
};

static uint8 block1[256]; //!< Workspace 1.
static uint8 block2[256]; //!< Worksapce 2.
static uint8 tempbuf[256];

static uint8 *powTbl; //!< Final location of exponentiation lookup table.
static uint8 *logTbl; //!< Final location of logarithm lookup table.
static uint8 *sBox; //!< Final location of s-box.
static uint8 *sBoxInv; //!< Final location of inverse s-box.
static uint8 *expandedKey; //!< Final location of expanded key.

static void CalcPowLog(uint8 *powTbl, uint8 *logTbl)
{
	uint8 i = 0;
	uint8 t = 1;

	do {
		// Use 0x03 as root for exponentiation and logarithms.
		powTbl[i] = t;
		logTbl[t] = i;
		i++;

		// Muliply t by 3 in GF(2^8).
		t ^= (t << 1) ^ (t & 0x80 ? BPOLY : 0);
	}while( t != 1 ); // Cyclic properties ensure that i < 255.

	powTbl[255] = powTbl[0]; // 255 = '-0', 254 = -1, etc.
}

static void CalcSBox( uint8 * sBox )
{
	uint8 i, rot;
	uint8 temp;
	uint8 result;

	// Fill all entries of sBox[].
	i = 0;
	do {
		//Inverse in GF(2^8).
		if( i > 0 )
		{
			temp = powTbl[ 255 - logTbl[i] ];
		}
		else
		{
			temp = 0;
		}

		// Affine transformation in GF(2).
		result = temp ^ 0x63; // Start with adding a vector in GF(2).
		for( rot = 0; rot < 4; rot++ )
		{
			// Rotate left.
			temp = (temp<<1) | (temp>>7);

			// Add rotated byte in GF(2).
			result ^= temp;
		}

		// Put result in table.
		sBox[i] = result;
	} while( ++i != 0 );
}

static void CalcSBoxInv( uint8 * sBox, uint8 * sBoxInv )
{
	uint8 i = 0;
	uint8 j = 0;

	// Iterate through all elements in sBoxInv using  i.
	do {
	// Search through sBox using j.
		do {
			// Check if current j is the inverse of current i.
			if( sBox[ j ] == i )
			{
				// If so, set sBoxInc and indicate search finished.
				sBoxInv[ i ] = j;
				j = 255;
			}
		} while( ++j != 0 );
	} while( ++i != 0 );
}

static void CycleLeft( uint8 * row )
{
	// Cycle 4 bytes in an array left once.
	uint8 temp = row[0];

	row[0] = row[1];
	row[1] = row[2];
	row[2] = row[3];
	row[3] = temp;
}

static void InvMixColumn( uint8 * column )
{
	uint8 r0, r1, r2, r3;

	r0 = column[1] ^ column[2] ^ column[3];
	r1 = column[0] ^ column[2] ^ column[3];
	r2 = column[0] ^ column[1] ^ column[3];
	r3 = column[0] ^ column[1] ^ column[2];

	column[0] = (column[0] << 1) ^ (column[0] & 0x80 ? BPOLY : 0);
	column[1] = (column[1] << 1) ^ (column[1] & 0x80 ? BPOLY : 0);
	column[2] = (column[2] << 1) ^ (column[2] & 0x80 ? BPOLY : 0);
	column[3] = (column[3] << 1) ^ (column[3] & 0x80 ? BPOLY : 0);

	r0 ^= column[0] ^ column[1];
	r1 ^= column[1] ^ column[2];
	r2 ^= column[2] ^ column[3];
	r3 ^= column[0] ^ column[3];

	column[0] = (column[0] << 1) ^ (column[0] & 0x80 ? BPOLY : 0);
	column[1] = (column[1] << 1) ^ (column[1] & 0x80 ? BPOLY : 0);
	column[2] = (column[2] << 1) ^ (column[2] & 0x80 ? BPOLY : 0);
	column[3] = (column[3] << 1) ^ (column[3] & 0x80 ? BPOLY : 0);

	r0 ^= column[0] ^ column[2];
	r1 ^= column[1] ^ column[3];
	r2 ^= column[0] ^ column[2];
	r3 ^= column[1] ^ column[3];

	column[0] = (column[0] << 1) ^ (column[0] & 0x80 ? BPOLY : 0);
	column[1] = (column[1] << 1) ^ (column[1] & 0x80 ? BPOLY : 0);
	column[2] = (column[2] << 1) ^ (column[2] & 0x80 ? BPOLY : 0);
	column[3] = (column[3] << 1) ^ (column[3] & 0x80 ? BPOLY : 0);

	column[0] ^= column[1] ^ column[2] ^ column[3];
	r0 ^= column[0];
	r1 ^= column[0];
	r2 ^= column[0];
	r3 ^= column[0];

	column[0] = r0;
	column[1] = r1;
	column[2] = r2;
	column[3] = r3;
}

static void SubBytes( uint8 * bytes, uint8 count )
{
	do {
		*bytes = sBox[ *bytes ]; // Substitute every byte in state.
		bytes++;
	} while( --count );
}

static void InvSubBytesAndXOR( uint8 * bytes, uint8 * key, uint8 count )
{
	do {
		// *bytes = sBoxInv[ *bytes ] ^ *key; // Inverse substitute every byte in state and add key.
		*bytes = block2[ *bytes ] ^ *key; // Use block2 directly. Increases speed.
		bytes++;
		key++;
	} while( --count );
}

static void InvShiftRows( uint8 * state )
{
	uint8 temp;

	// Note: State is arranged column by column.

	// Cycle second row right one time.
	temp = state[ 1 + 3*4 ];
	state[ 1 + 3*4 ] = state[ 1 + 2*4 ];
	state[ 1 + 2*4 ] = state[ 1 + 1*4 ];
	state[ 1 + 1*4 ] = state[ 1 + 0*4 ];
	state[ 1 + 0*4 ] = temp;

	// Cycle third row right two times.
	temp = state[ 2 + 0*4 ];
	state[ 2 + 0*4 ] = state[ 2 + 2*4 ];
	state[ 2 + 2*4 ] = temp;
	temp = state[ 2 + 1*4 ];
	state[ 2 + 1*4 ] = state[ 2 + 3*4 ];
	state[ 2 + 3*4 ] = temp;

	// Cycle fourth row right three times, ie. left once.
	temp = state[ 3 + 0*4 ];
	state[ 3 + 0*4 ] = state[ 3 + 1*4 ];
	state[ 3 + 1*4 ] = state[ 3 + 2*4 ];
	state[ 3 + 2*4 ] = state[ 3 + 3*4 ];
	state[ 3 + 3*4 ] = temp;
}

static void InvMixColumns( uint8 * state )
{
	InvMixColumn( state + 0*4 );
	InvMixColumn( state + 1*4 );
	InvMixColumn( state + 2*4 );
	InvMixColumn( state + 3*4 );
}

static void XORBytes( uint8 * bytes1, uint8 * bytes2, uint8 count )
{
	do {
		*bytes1 ^= *bytes2; // Add in GF(2), ie. XOR.
		bytes1++;
		bytes2++;
	} while( --count );
}

static void CopyBytes( uint8 * to, uint8 * from, uint8 count )
{
	do {
		*to = *from;
		to++;
		from++;
	} while( --count );
}

static void KeyExpansion( uint8 * expandedKey )
{
	uint8 temp[4];
	uint8 i;
	uint8 Rcon[4] = { 0x01, 0x00, 0x00, 0x00 }; // Round constant.

	uint8 * key = AES_Key_Table;

	// Copy key to start of expanded key.
	i = KEYLENGTH;
	do {
		*expandedKey = *key;
		expandedKey++;
		key++;
	} while( --i );

	// Prepare last 4 bytes of key in temp.
	expandedKey -= 4;
	temp[0] = *(expandedKey++);
	temp[1] = *(expandedKey++);
	temp[2] = *(expandedKey++);
	temp[3] = *(expandedKey++);

	// Expand key.
	i = KEYLENGTH;
	while( i < BLOCKSIZE*(ROUNDS+1) )
	{
		// Are we at the start of a multiple of the key size?
		if( (i % KEYLENGTH) == 0 )
		{
			CycleLeft( temp ); // Cycle left once.
			SubBytes( temp, 4 ); // Substitute each byte.
			XORBytes( temp, Rcon, 4 ); // Add constant in GF(2).
			*Rcon = (*Rcon << 1) ^ (*Rcon & 0x80 ? BPOLY : 0);
		}

		// Keysize larger than 24 bytes, ie. larger that 192 bits?
		#if KEYLENGTH > 24
		// Are we right past a block size?
		else if( (i % KEYLENGTH) == BLOCKSIZE ) {
		SubBytes( temp, 4 ); // Substitute each byte.
		}
		#endif

		// Add bytes in GF(2) one KEYLENGTH away.
		XORBytes( temp, expandedKey - KEYLENGTH, 4 );

		// Copy result to current 4 bytes.
		*(expandedKey++) = temp[ 0 ];
		*(expandedKey++) = temp[ 1 ];
		*(expandedKey++) = temp[ 2 ];
		*(expandedKey++) = temp[ 3 ];

		i += 4; // Next 4 bytes.
	}
}

static void InvCipher( uint8 * block, uint8 * expandedKey )
{
	uint8 round = ROUNDS-1;
	expandedKey += BLOCKSIZE * ROUNDS;

	XORBytes( block, expandedKey, 16 );
	expandedKey -= BLOCKSIZE;

	do {
		InvShiftRows( block );
		InvSubBytesAndXOR( block, expandedKey, 16 );
		expandedKey -= BLOCKSIZE;
		InvMixColumns( block );
	} while( --round );

	InvShiftRows( block );
	InvSubBytesAndXOR( block, expandedKey, 16 );
}





static uint8 Multiply( uint8 num, uint8 factor )
{
	uint8 mask = 1;
	uint8 result = 0;

	while( mask != 0 )
	{
	// Check bit of factor given by mask.
		if( mask & factor )
		{
		  // Add current multiple of num in GF(2).
		  result ^= num;
		}

		// Shift mask to indicate next bit.
		mask <<= 1;

		// Double num.
		num = (num << 1) ^ (num & 0x80 ? BPOLY : 0);
	}

	return result;
}

static uint8 DotProduct( uint8 * vector1, uint8 * vector2 )
{
	uint8 result = 0;

	result ^= Multiply( *vector1++, *vector2++ );
	result ^= Multiply( *vector1++, *vector2++ );
	result ^= Multiply( *vector1++, *vector2++ );
	result ^= Multiply( *vector1  , *vector2   );

	return result;
}

static void MixColumn( uint8 * column )
{
	uint8 row[8] = {0x02, 0x03, 0x01, 0x01, 0x02, 0x03, 0x01, 0x01};
	// Prepare first row of matrix twice, to eliminate need for cycling.

	uint8 result[4];

	// Take dot products of each matrix row and the column vector.
	result[0] = DotProduct( row+0, column );
	result[1] = DotProduct( row+3, column );
	result[2] = DotProduct( row+2, column );
	result[3] = DotProduct( row+1, column );

	// Copy temporary result to original column.
	column[0] = result[0];
	column[1] = result[1];
	column[2] = result[2];
	column[3] = result[3];
}

static void MixColumns( uint8 * state )
{
	MixColumn( state + 0*4 );
	MixColumn( state + 1*4 );
	MixColumn( state + 2*4 );
	MixColumn( state + 3*4 );
}

static void ShiftRows( uint8 * state )
{
	uint8 temp;

	// Note: State is arranged column by column.

	// Cycle second row left one time.
	temp = state[ 1 + 0*4 ];
	state[ 1 + 0*4 ] = state[ 1 + 1*4 ];
	state[ 1 + 1*4 ] = state[ 1 + 2*4 ];
	state[ 1 + 2*4 ] = state[ 1 + 3*4 ];
	state[ 1 + 3*4 ] = temp;

	// Cycle third row left two times.
	temp = state[ 2 + 0*4 ];
	state[ 2 + 0*4 ] = state[ 2 + 2*4 ];
	state[ 2 + 2*4 ] = temp;
	temp = state[ 2 + 1*4 ];
	state[ 2 + 1*4 ] = state[ 2 + 3*4 ];
	state[ 2 + 3*4 ] = temp;

	// Cycle fourth row left three times, ie. right once.
	temp = state[ 3 + 3*4 ];
	state[ 3 + 3*4 ] = state[ 3 + 2*4 ];
	state[ 3 + 2*4 ] = state[ 3 + 1*4 ];
	state[ 3 + 1*4 ] = state[ 3 + 0*4 ];
	state[ 3 + 0*4 ] = temp;
}

static void Cipher( uint8 * block, uint8 * expandedKey )
{
	uint8 round = ROUNDS-1;

	XORBytes( block, expandedKey, 16 );
	expandedKey += BLOCKSIZE;

	do {
		SubBytes( block, 16 );
		ShiftRows( block );
		MixColumns( block );
		XORBytes( block, expandedKey, 16 );
		expandedKey += BLOCKSIZE;
	} while( --round );

	SubBytes( block, 16 );
	ShiftRows( block );
	XORBytes( block, expandedKey, 16 );
}

static void aesEncInit(void)
{
	powTbl = block1;
	logTbl = tempbuf;
	CalcPowLog( powTbl, logTbl );

	sBox = block2;
	CalcSBox( sBox );

	expandedKey = block1;
	KeyExpansion( expandedKey );
}
static void aesDecInit(void)
{
	powTbl = block1;
	logTbl = block2;
	CalcPowLog( powTbl, logTbl );

	sBox = tempbuf;
	CalcSBox( sBox );

	expandedKey = block1;
	KeyExpansion( expandedKey );

	sBoxInv = block2; // Must be block2.
	CalcSBoxInv( sBox, sBoxInv );
}


 static void aesEncrypt( uint8 * buffer )
{
    uint8 chainBlock[BLOCKSIZE*2] = {0};
    aesEncInit();  //在执行加密初始化之前可以为AES_Key_Table赋值有效的密码数据
	XORBytes( buffer, chainBlock, BLOCKSIZE );
	Cipher( buffer, expandedKey );
	CopyBytes( chainBlock, buffer, BLOCKSIZE );
}

static void aesDecrypt( uint8 * buffer )
{
	uint8 temp[ BLOCKSIZE ];
	uint8 chainBlock[BLOCKSIZE*2] = {0};
    aesDecInit();   //在执行解密初始化之前可以为AES_Key_Table赋值有效的密码数据
	CopyBytes( temp, buffer, BLOCKSIZE );
	InvCipher( buffer, expandedKey );
	XORBytes( buffer, chainBlock, BLOCKSIZE );
	CopyBytes( chainBlock, temp, BLOCKSIZE );
}
/************************************************************************************************
* Function Name: AES_Encrypt
* Decription   : 加密操作
* Input        : uint8 * src    : 源地址
*                uint32  srcLen : 要加密的长度
*                uint8* des     : NULL目的地址，此处没有用到！！做参数兼容留个位置
*                uint32* key    : 密钥
* Output       : None
* Return       : src:正常     NULL:失败
* Others       : 注意，目的地址的长度满足俩个条件：不能少于源地址长度，且是8字节的整数倍
************************************************************************************************/
uint8  *AES_EncryptData(uint8 * src,uint32 srcLen,uint8* des,uint8* key)
{
    uint32 i = 0,EncryptLen  = srcLen;

    if(srcLen % BLOCKSIZE){
		EncryptLen = (srcLen/BLOCKSIZE + 1) * BLOCKSIZE;    //加密的长度定是8字节的整数倍
    }

    for(i = 0;i <(KEYBITS/8);i ++){
		AES_Key_Table[i] = key[i];  //密钥传输
    }

    for(i = 0;i < EncryptLen;i += BLOCKSIZE){
		aesEncrypt(&src[i]);
    }

	return src;
}
/************************************************************************************************
* Function Name: AES_DecryptData
* Decription   : 解密操作
* Input        : uint8 * src    : 源地址
*                uint32  srcLen : 要加密的长度
*                uint8* des     : NULL目的地址，此处没有用到！！做参数兼容留个位置
                 uint32* key    : 密钥
* Output       : None
* Return       : des:正常     NULL:失败
* Others       : 注意，目的地址的长度满足俩个条件：不能少于源地址长度，且是8字节的整数倍
************************************************************************************************/
uint8 *AES_DecryptData(uint8 * src,uint32 srcLen,uint8* des,uint8* key)
{
    uint32 i = 0,DecryptLen  = srcLen;

    if(srcLen % BLOCKSIZE){
      DecryptLen = (srcLen/BLOCKSIZE + 1) * BLOCKSIZE;    //加密的长度定是8字节的整数倍
    }

    for(i = 0;i <(KEYBITS/8);i ++){
		AES_Key_Table[i] = key[i];  //密钥传输
    }

	for(i = 0;i < DecryptLen;i += BLOCKSIZE){
		aesDecrypt(&src[i]);
    }

	return src;
}
#if 0
void AES_Test(void)
{
	uint8 i,dat[32]="0123456789ABCDEFG<>?;'[]",key[16] = {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16};
    printf("加密前：");for( i = 0;i < 32; i ++)printf("%02x",dat[i]);printf("\n");

	AES_EncryptData(dat,sizeof(dat),NULL,key);//AES加密，数组dat里面的新内容就是加密后的数据。
	//aesEncrypt(dat);
    printf("加密后：");for( i = 0;i < 32; i ++)printf("%02x",dat[i]);printf("\n");printf("%s\n",dat);

	AES_DecryptData(dat,sizeof(dat),NULL,key);//AES解密，密文数据存放在dat里面，经解密就能得到之前的明文。
	//aesDecrypt(dat);
	printf("解密后：");for( i = 0;i < 32; i ++)printf("%02x",dat[i]);printf("\n");printf("%s\n",dat);
}
#endif