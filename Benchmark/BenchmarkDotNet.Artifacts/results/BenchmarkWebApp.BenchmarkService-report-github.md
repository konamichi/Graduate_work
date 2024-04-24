```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3447/23H2/2023Update/SunValley3)
13th Gen Intel Core i7-13700F, 1 CPU, 24 logical and 16 physical cores
.NET SDK 8.0.200
  [Host]     : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  Job-QVCBPT : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2

InvocationCount=1  IterationCount=1  RunStrategy=Monitoring  
UnrollFactor=1  

```
| Method               | Mean       | Error |
|--------------------- |-----------:|------:|
| RunLoadArticles      | 1,415.9 ms |    NA |
| RunGetArticles       |   409.4 ms |    NA |
| RunSearch            |   402.1 ms |    NA |
| RunPublishArticle    |   375.7 ms |    NA |
| RunDeleteArticle     |   442.7 ms |    NA |
| RunEditArticle       |   426.1 ms |    NA |
| RunGetCategoryByName |   422.7 ms |    NA |
| RunGetCategoryById   |   417.5 ms |    NA |
