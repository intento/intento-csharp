using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentoSDK
{
    // Revision history
    // 1.1.0: Public version
    // 1.1.1: 2019-01-06
    //   - Corrected behaviour for async-wait-async request with sandbox apikey or error in validation of prarmeters.
    //     In this case no operation id returned from IntentoAPI, but result (in case of sandbox key) or error information.
    // 1.1.2: 2019-02-02
    //   - More correct processing of text parameter of Fulfill operation
    // 1.1.3: 2019-03-31
    //   - Added methods for obtaining models and glossaries from the provider
    // 1.2.1: 2019-05-28
    //   - Smart delays in wait-async mode
    // 1.2.2: 2019-06-08
    //   - Logging callback.
    //   - Call logging callback for successfull and non-susscessfull Intento API calls.
    //   - try-catch around calls to http to write log.
    // 1.2.3: 2019-06-25
    // - Bug with extracting version from dll
    // 1.2.4: 2019-07-02
    // - waitAsyncDelay
    // 1.2.5: 2019-07-03
    // - The version in useragent now has a commit hash in git
    // - Auxiliary public methods for extracting information from the assembly
    // 1.2.6: 2019-07-14
    // - Minor changes in processing of Modes and Glossaries operations.
    // 1.2.7: 2019-08-14
    // - Change version of Newtonsoft dll to be compatible with other products (downgrade to 10.0.3)
    // 1.3.0: 2019-09-15
    // - http(s) proxy support
    // 1.3.1: 2019-10-08
    // - Minor changes in Models and Glossaries calls
    // 1.4.0: 2019-10-08
    // - IDispose, Serialization for Exceptions, using, etc (from memoQ)
    // 1.4.1: 2019-11-14
    // - HttpStatusCode.Unauthorized is interpreted as invalid ApiKey
    // 1.4.2: 2019-12-14
    // - Version for memoQ 9.3
    // 1.4.3: 2020-03-12
    // - Demo
    // - bug in Fulfill parameter
    // 1.4.4: 2020-04-15
    // - special_headers parameter in Fulfill
    // 1.4.5: 2020-08-11
    // - Extended demo
    // - -1 as a special value for delay parameter of WaitAsyncJob
    // 1.5.0: 2020-09-25
    // - Added Accounts method
    // - additionalParams now Dictionary<string, string>
    // 1.5.1: 2020-12-23
    // - Added PairsAsync and Pairs methods: getting pairs for routing from ai/text/translate/routing/.../pairs

}
