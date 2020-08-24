Template Method is based on inheritance: it lets you alter parts of an algorithm by extending those parts in subclasses. Strategy is based on composition: you can alter parts of the object’s behavior by supplying it with different strategies that correspond to that behavior. Template Method works at the class level, so it’s static. Strategy works on the object level, letting you switch behaviors at runtime.


Ett annat bra exempel:
- DataMiner med underklasser
  - DocDataMiner
  - CSVDataMiner
  - PDFDataMiner

Steg:
1) Öppna fil
2) Extrahera data
3) Parsea data
4) Analysera data
5) Skicka en rapport
6) Stäng filen