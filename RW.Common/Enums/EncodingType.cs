using System.ComponentModel;
using System.Text;

namespace RW.Common.Enums;

public enum EncodingType {
	UTF8,
	UTF7,
	UTF32,
	GB2312,
	GBK,
	GB18030,
	Big5,
	Unicode,
	BigEndianUnicode,
	ASCII,
	[Description("ISO-8859-15")] ISO_8859_15,
}

public static class EncodingTypeHelper {
	public static Encoding ConvertEncoding(this EncodingType type) {
		return type switch {
			EncodingType.UTF8 => Encoding.UTF8,
			EncodingType.UTF7 => Encoding.UTF7,
			EncodingType.UTF32 => Encoding.UTF32,
			EncodingType.GB2312 => Encoding.GetEncoding("gb2312"),
			EncodingType.Unicode => Encoding.Unicode,
			EncodingType.BigEndianUnicode => Encoding.BigEndianUnicode,
			EncodingType.ASCII => Encoding.ASCII,
			EncodingType.GBK => Encoding.GetEncoding("GBK"),
			EncodingType.GB18030 => Encoding.GetEncoding("GB18030"),
			EncodingType.Big5 => Encoding.GetEncoding("Big5"),
			EncodingType.ISO_8859_15 => Encoding.GetEncoding("ISO-8859-15"),
			_ => throw new NotImplementedException(),
		};
	}
}
