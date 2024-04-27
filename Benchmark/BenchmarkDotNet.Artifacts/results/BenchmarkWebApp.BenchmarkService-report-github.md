```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3447/23H2/2023Update/SunValley3)
13th Gen Intel Core i7-13700F, 1 CPU, 24 logical and 16 physical cores
.NET SDK 8.0.200
  [Host]     : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  Job-UFVXYO : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2

InvocationCount=1  IterationCount=1  RunStrategy=Monitoring  
UnrollFactor=1  

```
| Method               | Mean       | Error |
|--------------------- |-----------:|------:|
| RunLoadArticles      | 1,644.2 ms |    NA |
| RunGetArticles       |   372.1 ms |    NA |
| RunSearch            |   366.0 ms |    NA |
| RunPublishArticle    |   359.4 ms |    NA |
| RunDeleteArticle     |   421.0 ms |    NA |
| RunEditArticle       |   411.4 ms |    NA |
| RunGetCategoryByName |   375.2 ms |    NA |
| RunGetCategoryById   |   383.6 ms |    NA |
