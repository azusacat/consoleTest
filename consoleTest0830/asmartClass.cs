using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using System.Text;


namespace consoleTest0830
{
    public class Asmart
	{
		public class aProd : IEquatable<aProd>
		{
			public int id { get; set; }
			public string title { get; set; }
			public string name { get; set; }
			public string stock_message { get; set; }

			public override string ToString()
			{
				return stock_message + " ID : " + id + "   Title: " + title + "   Name: " + name;
			}
			public override bool Equals(object obj)
			{
				if (obj == null) return false;
				aProd objAsPart = obj as aProd;
				if (objAsPart == null) return false;
				else return Equals(objAsPart);
			}
			public override int GetHashCode()
			{
				return id;
			}
			public bool Equals(aProd other)
			{
				if (other == null) return false;
				return (this.id.Equals(other.id));
			}
			// Should also override == and != operators.

		}

		public static string[] replaceString = {
		};
		public const string AsmartJson = "http://104.223.65.182/bm/as.php?type=json";
		public static string[] AsmartUrl = {
			"http://www.asmart.jp/Form/Product/ProductList.aspx?shop=0&cat=100131&swrd=&sort=3&img=2&dcnt=60&search_kbn=&price_from=&price_to=&filter_new=&filter_reserve=&filter_privilege=&filter_fc=&saleable_kbn=0&genre=&sign=&campaign=&lang=JP",
			"http://www.asmart.jp/Form/Product/ProductList.aspx?shop=0&cat=100131&swrd=&sort=3&img=2&dcnt=60&search_kbn=&price_from=&price_to=&filter_new=&filter_reserve=&filter_privilege=&filter_fc=&saleable_kbn=0&genre=&sign=&campaign=&lang=JP&pno=2",
			"http://www.asmart.jp/Form/Product/ProductList.aspx?shop=0&cat=100131&swrd=&sort=3&img=2&dcnt=60&search_kbn=&price_from=&price_to=&filter_new=&filter_reserve=&filter_privilege=&filter_fc=&saleable_kbn=0&genre=&sign=&campaign=&lang=JP&pno=3",
		};

		// public static Dictionary<string, Dictionary<string, string>> getServerJson()
		public static List<aProd> getServerJson()
		{
            Console.WriteLine(@"Loading Asmart data...");
			//List<string> resStockList = new List<string>();
			//Dictionary<string, Dictionary<string, string>> resStockList = new Dictionary<string, Dictionary<string, string>>();

			List<aProd> resStockList = new List<aProd>();

			for (int a = 0; a < AsmartUrl.Length; a = a + 1)
			{
				string data = "";
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(AsmartUrl[a]);
				//Console.WriteLine(AsmartUrl[a]);
				request.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_5) AppleWebKit/603.2.4 (KHTML, like Gecko) Version/10.1.1 Safari/603.2.4";
				request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
				//  request.ContentType = "application/x-www-form-urlencoded";
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();

				if (response.StatusCode == HttpStatusCode.OK)
				{
					Stream receiveStream = response.GetResponseStream();
					StreamReader readStream = null;

					if (response.CharacterSet == null)
					{
						readStream = new StreamReader(receiveStream);
					}
					else
					{
						readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
					}

					data = readStream.ReadToEnd();

					response.Close();
					readStream.Close();

					//實例化HtmlAgilityPack.HtmlDocument對像
					HtmlDocument doc = new HtmlDocument();
					//載入HTML
					doc.LoadHtml(data);
					foreach (HtmlNode dl in doc.DocumentNode.SelectNodes("//dl[@class='list']"))
					{
						var prodId = 0;
						var prodTitle = "";
						var prodName = "";
						var prodStockMsg = "";
						foreach (HtmlNode span in dl.SelectNodes(".//span"))
						{
							var span_id = span.GetAttributeValue("id", "");
							var span_class = span.GetAttributeValue("class", "");

							//Cat
							if (span_class == @"tit_cat")
							{
								prodTitle = realString(span.InnerHtml);
								// Console.WriteLine(realString(span.InnerHtml));

							}
							//Name
							if (span_class == @"tit")
							{
								//resStockList.Add(realString(span.SelectSingleNode(".//a").InnerHtml));
								prodName = realString(span.SelectSingleNode(".//a").InnerHtml);
								//Console.WriteLine(realString(span.SelectSingleNode(".//a").InnerHtml));

							}

							//Stock Message
							if (span_id.IndexOf("_spStockMessage") > 0)
							{
								prodStockMsg = realString(span.InnerHtml);
								//Console.WriteLine(realString(span.InnerHtml));
							}
						}
						//  Console.WriteLine(listValue.Values.ToString());
						resStockList.Add(new aProd()
						{
							id = prodId,
							title = prodTitle,
							name = prodName,
							stock_message = prodStockMsg
						});
						// = "crank arm", PartId = 1234 });
						// resStockList.Add(listValue.name , listValue);
						// listValue.Values.CopyTo(resStockList);
						// resStockList.Add(listValue.Values.ToString());
					}
				}
			}


			// Console.WriteLine(JsonConvert.SerializeObject(resStockList));
			//Console.WriteLine(resStockList);
			return resStockList;
		}

		public static string realString(string html)
		{
			return html.Replace("\r", "").Replace("\n", "").Trim();
		}

		public static string strPad(string org, string RL, int sLen = 30)
		{
			var sResult = "";
			//計算轉換過實際的總長
			int orgLen = 0;
			int tLen = 0;
			for (int i = 0; i < org.Length; i++)
			{
				string s = org.Substring(i, 1);
				int vLen = 0;
				//判斷 asc 表是否介於 0~128
				if (Convert.ToInt32(s[0]) > 255 || Convert.ToInt32(s[0]) < 0)
				{
					vLen = 2;
				}
				else
				{
					vLen = 1;
				}
				orgLen += vLen;
				if (orgLen > sLen)
				{
					orgLen -= vLen;
					break;
				}
				sResult += s;
			}
			//計算轉換過後，最後實際的長度
			tLen = sLen - (orgLen - org.Length);
			if (RL == "R")
			{
				return sResult.PadLeft(tLen);
			}
			else
			{
				return sResult.PadRight(tLen);
			}
			//return text.PadRight(textLen - Encoding.GetEncoding("big5").GetBytes(text).Length);
		}
		public static void listToTable(List<aProd> resStockList)
		{
			Console.Clear();
            Console.WriteLine(@"Time : " + DateTime.Now.ToString());
			Console.WriteLine("|".PadRight(160, '-') + "|");
			Console.WriteLine("|{0}\t|{1}\t|{2}\t|", strPad("Stock", "L", 20), strPad("Title", "L", 30), strPad("Name", "L", 100));
			Console.WriteLine("|".PadRight(160, '-') + "|");
            //List<aProd> SortedList = resStockList.OrderByDescending(o => o.stock_message).ToList();
			resStockList.Sort((x, y) => -1 * x.stock_message.CompareTo(y.stock_message));
            foreach (aProd aPart in resStockList)
			{
                Console.WriteLine("|{0}\t|{1}\t|{2}\t|", strPad(aPart.stock_message, "L", 20), strPad(aPart.title, "L", 30), strPad(aPart.name, "L", 100));
			}
			Console.WriteLine("|".PadRight(160, '-') + "|");
		}
    }
}
