#ifndef __AES_H_
#define __AES_H_

uint8  *AES_EncryptData(uint8 * src,uint32 srcLen,uint8* des,uint8* key);
uint8  *AES_DecryptData(uint8 * src,uint32 srcLen,uint8* des,uint8* key);
void AES_Test(void);


#endif // __AES_H_
