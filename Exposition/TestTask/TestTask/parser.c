#include <stdio.h>
#include "parser.h"

#define MASK_LENGTH 64
#define BUFFER_SIZE 200
#define NOT_INITIALIZED 2
#define REQUIRED_CHANGES 4
#define BLOCKS_AMOUNT 2048
#define ID_USED 1

typedef struct tagBlock
{
  char oldMask[MASK_LENGTH], newMask[MASK_LENGTH], changesCounter[MASK_LENGTH], probablyHaveWantedBit;
  uint actualBitLength;
} Block;

void showResult( Block block, int id )
{
  uint i;

  for (i = 0; i < block.actualBitLength; ++i)
  {
    if (block.changesCounter[i] == REQUIRED_CHANGES)
    {
      printf_s("ID: %i\nByte num: %i\nBit num: %i\n", id, i / 8 + 1, i % 8 + 1);
      return;
    }
  }
}

int searchForBlockWanted( Block * blocks, int * idWasUsed )
{
  uint i;

  for (i = 0; i < BLOCKS_AMOUNT; ++i)
  {
    if (idWasUsed[i] == ID_USED)
    {
      if (blocks[i].probablyHaveWantedBit == 1)
      {
        showResult(blocks[i], i);
        return SUCCESS_RETURN_VALUE;
      }
    }
  }
  return ERROR_RETURN_VALUE;
}

void markBits( Block * block, int id )
{
  uint i;
  static uchar isFirstTime[BLOCKS_AMOUNT] = { 0 };

  /* marking 100% wrong bits that were 1 on the very start */
  if (isFirstTime[id] == 0)
  {
    for (i = 0; i < block[id].actualBitLength; ++i)
    {
      block[id].oldMask[i] = block[id].newMask[i];
      if (block[id].newMask[i] == 1)
      {
        block->changesCounter[i] = REQUIRED_CHANGES + 1;
      }
    }
    ++isFirstTime[id];
    return;
  }


  for (i = 0; i < block[id].actualBitLength; ++i)
  {
    if (block[id].oldMask[i] != block[id].newMask[i] && block[id].changesCounter[i] < REQUIRED_CHANGES + 1)
    {
      if (++block[id].changesCounter[i] == REQUIRED_CHANGES)
      {
        ++block[id].probablyHaveWantedBit;
      }
      else if (block[id].changesCounter[i] > REQUIRED_CHANGES)
      {
        --block[id].probablyHaveWantedBit;
      }
    }
    block[id].oldMask[i] = block[id].newMask[i];
  }

}

int parseString( const char * source, Block * block )
{
  uint curIndex = 0, byteIndexer, i, maskIndexer, dataSize;
  int ID;

  if ((ID = hexToInt(source, &curIndex)) == ERROR_RETURN_VALUE)
  {
    return ERROR_RETURN_VALUE;
  }

  runThroughWhitespaces(source, &curIndex);

  dataSize = source[curIndex++] - '0';
  block[ID].actualBitLength = dataSize * 8;
  if (dataSize < 1 || dataSize > 8)
  {
    return ERROR_RETURN_VALUE;
  }

  for (byteIndexer = 0, maskIndexer = 0; byteIndexer < dataSize; ++byteIndexer)
  {
    int byteValue = 0;

    if ((byteValue = hexToInt(source, &curIndex)) == ERROR_RETURN_VALUE)
    {
      return ERROR_RETURN_VALUE;
    }
    /* byte into array */
    for (i = 128; i > 0; i /= 2)
    {
      block[ID].newMask[maskIndexer++] = ((byteValue & i) != 0);
    }
  }

  return ID;
}

void parseLog( const char * logPath )
{
  uint i, isTargetFile = 0;
  int id;
  int idInUse[BLOCKS_AMOUNT] = { 0 };
  Block blocks[BLOCKS_AMOUNT] = { 0 };
  char parseBuffer[BUFFER_SIZE];
  FILE *logFile;

  if (fopen_s(&logFile, logPath, "r") != 0)
  {
    perror("File wasn't found.");
    return;
  }

  while (fgets(parseBuffer, BUFFER_SIZE, logFile) != NULL)
  {
    /* run through senseless start of string */
    for (i = 1; parseBuffer[i] != 0 && parseBuffer[i] != 'x'; ++i)
    {
      ;
    }
    if (parseBuffer[i] != 0)
    {
      ++isTargetFile;
      if ((id = parseString(&parseBuffer[i + 1], blocks)) == ERROR_RETURN_VALUE)
      {
        perror("Damaged file.");
        fclose(logFile);
        return;
      }
      idInUse[id] = ID_USED;
      markBits(blocks, id);
    }
  }
  if (isTargetFile == 0)
  {
    perror("Possibly wrong path!");
    return;
  }

  if (searchForBlockWanted(blocks, idInUse) == ERROR_RETURN_VALUE)
  {
    perror("Bit wasn't found. Possibly damaged file.");
  }

  fclose(logFile);
}
