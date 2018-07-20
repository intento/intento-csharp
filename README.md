# C# SDK

## Overview
The solution consist of two projects: 
- IntentoSDK: SDK itself as a library
- TestForm: simple test form application. 

You may provide your ApiKey in TestForm/Program.cs line 20. If you do not provide it, TestForm application will ask your ApiKey every time you run it. 

## Demo application
Sources and .sln of demo application are located in TestForm. Executable file is in TestForm/bin/debug. To try you need to download IntentoDSK.dll, Newtonsoft.Json.dll and TestForm.exe. To run TestForm.exe you need to have an apikey from Intento. 
To simplify using of this application you may save ApiKey into registry during first usage. 

## Making connection to Intento API
Firstly you need to make connection to Intento API:
```csharp
Intento intento = Intento.Create(apiKey)
```

## Making conection to translate intent
Obtain conection to translation intent: 
```csharp
IntentoAiTextTranslate translate = intento.Ai.Text.Translate;
```

## Fulfill
To call translate intent with translation request use Fullfill method: 
```csharp
dynamic result = translate.Fulfill(text, to, from: from, provider: provider, format: "html");
string text = result.results[0];
```
Parameters (json below means Newtonsoft JObject or JArray, json-string means serialized json string):
- text (required, string): text to translate
- to (required, string): language to translate, like "en", "es", "ru", etc. 
- from (optional, string): language from. If there is no parameter, then autodetect will be used.  
- provider (optional, string): provider to use, "ai.text.translate.yandex.translate_api.1-5" for example. If there is no parameter, then smart routing will be used.  
- format (optional, string): "html" if you want to translate text with html markup. 
- auth (optional, json-string or json): use own keys to access service provider. Format is described here (parameter auth): https://github.com/intento/intento-api#using-a-service-provider-with-your-own-keys
- custom_model (optional, string): use custom model as described here (parameter category): https://github.com/intento/intento-api/blob/master/ai.text.translate.md#using-previously-trained-custom-models
- pre_processing, post_processing (optional, list of strings or json): use pre- and/or post-processing as described here (parameters processing.pre and processing.post): https://github.com/intento/intento-api/blob/master/ai.text.translate.md#content-processing
- failover (optional, bool), failover_list (optional, json-string or json), bidding (optional, string): use failover mode as described here: https://github.com/intento/intento-api#failover-mode
- async (optional, bool): async mode of processing. 
Returns: dynamic object structure as described here: https://github.com/intento/intento-api/blob/master/ai.text.translate.md#basic-usage 
Be careful! results field is always an array. 

In case of successful call you will have result in form of dynamic data with the following fields:
- results - array of text strings
- All other fields as described here: https://github.com/intento/intento-api/blob/master/ai.text.translate.md#basic-usage

In case of any problems you will get IntentoException, it contains StatusCode and other information. 

## Intento Async Mode (Fulfill)
Async Mode is described here: https://github.com/intento/intento-api#async-mode
Intento Async Mode starts with a call Fulfill with a subsequent call CheckAsyncJob. 
Fulfill call has an additional parameter: 
```csharp
dynamic result = translate.Fulfill(text, to, from: from, provider: provider, async: true);
string asyncId = result.id;
```

By calling CheckAsyncJob you may check is operation complete and to get results: 
```csharp
dynamic result = intento.CheckAsyncJob(asyncId);
if (result.done == true)
	text = result.response[0].results
```
If results.done is not true: operation is not completed yet and you must request results again later. 
Be careful! CheckAsyncJob method is in intento, not in translate. 

## Providers: Obtain a list of providers
```csharp
IList<dynamic>providers = translate.Providers();
```
Optional parameters: 
- to and from: if present filter providers on language pairs. 
Result: A list of items described each provider. Each item is an dynamic object with structure described in https://github.com/intento/intento-api/blob/master/ai.text.translate.md#getting-available-providers


## Languages: Obtain a list of supported languages
```csharp
IList<dynamic>languages = translate.Languages();
Result: A list of items described each language. Each item is an dynamic object with structure described in https://github.com/intento/intento-api/blob/master/ai.text.translate.md#list-of-supported-languages
```


## async-await
Do not confuse async-await practice to call any of Intento C# SDK methods with parameter async of Fulfill call. 
All API methods are available in synchronious and asynchronious versions, all asynchronious mathods has the same parameters but name ends with Async, like FulfillAsync. 
All synchronious versions are implemented by calling corresponding asynchronious version. 

