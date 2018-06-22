# C# SDK

## Overview
Solution consist of two projects: 
- IntentoSDK - SDK itself as a library
- TestForm - simple test form application. 

To use test application you need to provide your ApiKey in TestForm/Program.cs line 20.

## Making connection to Intento API
Firstly you need to make connection to Intento API:
```csharp
Intento intento = Intento.Create(apiKey)
```

## Creating conection to translate intent
To get conection to translation intent you need to obtain it: 
```csharp
IntentoAiTextTranslate translate = intento.Ai.Text.Translate;
```

## Fulfill
To call translate intent with translation request you need to use Fullfill method: 
```csharp
dynamic result = translate.Fulfill(text, to, from: from, provider: provider);
string text = result.results[0];
```
where:
- text - is a text to translate
- to - language to translate, like "en", "es", "ru", etc. 
- from - language from. 
- provider - provider to use, "ai.text.translate.yandex.translate_api.1-5" for example.

In case of successful call you will have result with the fillowing fields:
- results - array of text strings
- All other fields as described here: https://github.com/intento/intento-api/blob/master/ai.text.translate.md#basic-usage

In case of any problems you will get IntentoException, it contains StatusCode and other information. 

## Async mode
Async mode started with Fulfill call with following CheckAsyncJob call. 
Fulfill call has an additional parameter: 
```csharp
dynamic result = translate.Fulfill(text, to, from: from, provider: provider, async: true);
string asyncId = result.id;
```

This async id you must use to get results of operation: 
```csharp
dynamic result = intento.CheckAsyncJob(asyncId);
if (result.done)
	text = result.response[0].results
```
If results.don is not true operation is not completed yet and you must request resuts again later. 
Be careful! CheckAsyncJob methos is in intento, not in translate. 

## async-await
All API calls are available in synchronious and asynchronious versions, all asynchronious has the same parameters but name ends with Async. All synchronious versiona implemented by calling corresponding asynchronious version. 

