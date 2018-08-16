#include "utilities.h"

void runThroughWhitespaces(const char * source, uint * position)
{
  uint i = *position;

  for (; source[i] == ' ' || source[i] == '\t'; ++i)
  {
    ;
  }
  *position = i;
}

int hexToInt(const char * source, uint * position)
{
  uint i, result = 0;

  runThroughWhitespaces(source, position);

  if (source[*position] != 0 && source[*position] == '0' && source[*position + 1] == 'x')
  {
    *position += 2;
  }

  for (i = *position; source[i] != ' ' && source[i] != '\t' && source[i] != 0; ++i)
  {
    if (source[i] < '0' || source[i] > 'F' || (source[i] > '9' && source[i] < 'A'))
    {
      return ERROR_RETURN_VALUE;
    }
    result *= 16;
    if (source[i] - '0' < 10)
    {
      result += source[i] - '0';
    }
    else
    {
      result += source[i] - 'A' + 10;
    }
  }
  if (*position == i)
  {
    return ERROR_RETURN_VALUE;
  }
  *position = i;

  return result;
}