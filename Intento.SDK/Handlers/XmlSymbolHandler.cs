﻿using System;
using System.Collections.Generic;

namespace Intento.SDK.Handlers
{
    /// <summary>
    /// Xml symbols prepare
    /// </summary>
    internal class XmlSymbolHandler : BaseSymbolHandler
    {
        protected override IReadOnlyDictionary<string, string> SpecialCodesIn => new Dictionary<string, string>
         {
           { "&amp;gt;" ,   "<tph1/>" },
           { "&amp;lt;" ,   "<tph2/>" },
           { "&lt;"     ,   "<tph3/>" },
           { "&gt;"     ,   "<tph4/>" }
         };

        protected override IReadOnlyDictionary<string, string> SpecialCodesOut => new Dictionary<string, string>
        {
           { "<tph1>" ,   "&amp;gt;"  },
           { "<tph2>" ,   "&amp;lt;"  },
           { "<tph3>" ,   "&lt;"      },
           { "<tph4>" ,   "&gt;"      },
           { "<tph1/>",   "&amp;gt;"  },
           { "<tph2/>",   "&amp;lt;"  },
           { "<tph3/>",   "&lt;"      },
           { "<tph4/>",   "&gt;"      },
           { "<tph1 />",  "&amp;gt;"  },
           { "<tph2 />",  "&amp;lt;"  },
           { "<tph3 />",  "&lt;"      },
           { "<tph4 />",  "&gt;"      },
           { "</tph1>",   ""          },
           { "</tph2>",   ""          },
           { "</tph3>",   ""          },
           { "</tph4>",   ""          }
        };

        public override string Format => "xml";

        protected override string PrepareResult(string text)
        {
            text = base.PrepareResult(text);

            // Remove <? > tag
            var n1 = text.IndexOf("<?", StringComparison.Ordinal);
            var text2 = text;
            if (n1 != -1)
            {
                var n2 = text.IndexOf(">", StringComparison.Ordinal);
                text2 = text.Substring(n2 + 1);
            }

            // Remove <root> and </root> tags
            var text3 = text2.Replace("<root>", "").Replace("</root>", "");
            return text3;
        }

        protected override string PrepareText(string data)
        {
            data = base.PrepareText(data);
            return $"<root>{data}</root>";
        }
    }
}
