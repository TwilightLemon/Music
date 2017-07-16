using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace T
{
	public class M
	{
		public static void Main(string[] args)
		{
			while(true){
				try{
					Console.WriteLine ("选择模式:s(搜索模式)、i(ID模式)、z(歌单模式)");
			var si = Console.ReadLine ();
			if (si == "s") {
				int osx = 1;
				int i=0;
				List<string> v = new List<string> ();
				Console.WriteLine ("已选择搜索模式，请键入搜索关键词:");
				var gc = Console.ReadLine ();
				JObject o = JObject.Parse(GetWebAsync($"http://59.37.96.220/soso/fcgi-bin/client_search_cp?format=json&t=0&inCharset=GB2312&outCharset=utf-8&qqmusic_ver=1302&catZhida=0&p={osx}&n=50&w={gc}&flag_qc=0&remoteplace=sizer.newclient.song&new_json=1&lossless=0&aggr=1&cr=1&sem=0&force_zonghe=0"));
				while (i < o["data"]["song"]["list"].Count())
				{
					Music m = new Music();
					m.MusicName = o["data"]["song"]["list"][i]["name"].ToString();
					string Singer = "";
					for (int osxc = 0; osxc != o["data"]["song"]["list"][i]["singer"].Count(); osxc++)
					{ Singer += o["data"]["song"]["list"][i]["singer"][osxc]["name"] + "/"; }
					m.Singer = Singer.Substring(0, Singer.LastIndexOf("/"));
					m.ZJ = o["data"]["song"]["list"][i]["album"]["name"].ToString();
					m.MusicID = o["data"]["song"]["list"][i]["mid"].ToString();
					m.ImageID = o["data"]["song"]["list"][i]["album"]["mid"].ToString();
					m.GC = o["data"]["song"]["list"][i]["id"].ToString();
					m.Fotmat = o["data"]["song"]["list"][i]["file"]["size_flac"].ToString();
					m.HQFOTmat = o["data"]["song"]["list"][i]["file"]["size_ogg"].ToString();
					m.MV = o["data"]["song"]["list"][i]["mv"]["id"].ToString();
					string Q = "";
					if (m.Fotmat != "0")
						Q = "SQ";
					if (m.HQFOTmat != "0")
					if (m.Fotmat == "0")
						Q = "HQ";
					Console.WriteLine($"[{i}]  {m.MusicName}-{m.Singer}  {Q}");
					v.Add(m.MusicID);
					i++;
				}
				Console.WriteLine("搜索完成，键入序号播放");
				String id = v[int.Parse(Console.ReadLine ())];
				GetUri(id);
					} else if(si=="i"){
						Console.WriteLine ("已选择ID模式，请键入ID:");
				String id = Console.ReadLine ();
				GetUri(id);
					}else if(si=="z"){
						Console.WriteLine ("已选择歌单模式，请键入歌单ID:(留空为默认歌单TwilightMusicWorld)");
						String id = Console.ReadLine ();
						if(id == "")
							id = "2591355982" ;
						List<string> v = new List<string> ();
						var s = GetWebAsync($"https://y.qq.com/n/yqq/playlist/{id}.html#stat=y_new.profile.create_playlist.click&dirid=6");
						var j = "{\"list\":" + Text(s, "var getSongInfo = ", ";", 0) + "}";
						JObject o = JObject.Parse(j);
						int i = 0;
						while (i != o["list"].Count())
						{
							Music m = new Music()
							{
								MusicName = o["list"][i]["songname"].ToString(),
								Singer = o["list"][i]["singer"][0]["name"].ToString(),
								ZJ = o["list"][i]["albumdesc"].ToString(),
								GC = o["list"][i]["songid"].ToString(),
								Fotmat = o["list"][i]["sizeflac"].ToString(),
								HQFOTmat = o["list"][i]["size320"].ToString(),
								MusicID = o["list"][i]["songmid"].ToString(),
								ImageID = o["list"][i]["albummid"].ToString(),
								MV = o["list"][i]["vid"].ToString()
							};
							string Q = "";
							if (m.Fotmat != "0")
								Q = "SQ";
							if (m.HQFOTmat != "0")
							if (m.Fotmat == "0")
								Q = "HQ";
							Console.WriteLine($"[{i}]  {m.MusicName}-{m.Singer}  {Q}");
							v.Add(m.MusicID);
							i++;
						}
						Console.WriteLine("搜索完成，键入序号播放");
						String ide = v[int.Parse(Console.ReadLine ())];
						GetUri(ide);
					}
				}catch{Console.WriteLine("还有这种操作??!!");if(Console.ReadLine()=="就是有这种操作")Console.WriteLine("已经没有这种操作了");return;}
			}
		}

		public static void GetUri(string id){
			var uri = "";
			Console.WriteLine ("选择品质:a(经济)、b(标准)、c(高品质)");
			var pz = Console.ReadLine ();
			if (pz == "a")
				uri = $"http://cc.stream.qqmusic.qq.com/C100{id}.m4a?fromtag=52";
			else if (pz == "b") {
				string guid = "20D919A4D7700FBC424740E8CED80C5F";
				string ioo =  GetWebAsync($"http://59.37.96.220/base/fcgi-bin/fcg_musicexpress2.fcg?version=12&miniversion=92&key=19914AA57A96A9135541562F16DAD6B885AC8B8B5420AC567A0561D04540172E&guid={guid}");
				//	Console.WriteLine(ioo);
				string vkey = Text(ioo, "key=\"", "\" speedrpttype", 0);
				//	Console.WriteLine(vkey);
				uri = $"http://182.247.250.19/streamoc.music.tc.qq.com/M500{id}.mp3?vkey={vkey}&guid={guid}";
			}else if(pz=="c"){
				string guid = "20D919A4D7700FBC424740E8CED80C5F";
				string ioo =  GetWebAsync($"http://59.37.96.220/base/fcgi-bin/fcg_musicexpress2.fcg?version=12&miniversion=92&key=19914AA57A96A9135541562F16DAD6B885AC8B8B5420AC567A0561D04540172E&guid={guid}");
				//Console.WriteLine(ioo);
				string vkey = Text(ioo, "key=\"", "\" speedrpttype", 0);
				//Console.WriteLine(vkey);
				uri = $"http://182.247.250.19/streamoc.music.tc.qq.com/O600{id}.ogg?vkey={vkey}&guid={guid}";
			}
			var vu=GetWebAsync($"http://api.t.sina.com.cn/short_url/shorten.json?source=3271760578&url_long={Uri.EscapeDataString(uri)}");
			Process.Start (Text(vu,"\"url_short\":\"","\",\"",0));
			Console.ReadLine ();
		}

		public static string GetWebAsync(string url)
		{
			try
			{
				HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(url);
				var o = hwr.GetResponse();
				StreamReader sr = new StreamReader(o.GetResponseStream(), Encoding.UTF8);
				var st =  sr.ReadToEnd();
				sr.Dispose();
				return st;
			}
			catch { return ""; }
		}
		public static string Text(string all, string r, string l, int t)
		{

			int A = all.IndexOf(r, t);
			int B = all.IndexOf(l, A + 1);
			if (A < 0 || B < 0)
			{
				return null;
			}
			else
			{
				A = A + r.Length;
				B = B - A;
				if (A < 0 || B < 0)
				{
					return null;
				}
				return all.Substring(A, B);
			}

		}
	}
}

