#include <stdio.h>
#include "parser.h"

int main( int argc, char ** argv )
{
  if (argc < 2)
  {
    perror("Wrong command.");
    return -1;
  }

  parseLog(argv[1]);
  return 0;
}
