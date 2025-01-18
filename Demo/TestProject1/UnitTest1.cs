using RW.Common.Helpers;
using RW.Common.Models;
using System.ComponentModel;

namespace TestProject1;

public class Tests {
	[SetUp]
	public void Setup() {
		string? text = null;
		string v = text.NotBlankCheck() ?? "";
		if (text.IsNotBlank()) {
			string t = text;
		}

		List<int> list = [];
		int v2 = CollectionHelper.GetFromIndexOrDefault(list, 1);

		//ExtendReadonlyList<string> s = new ExtendReadonlyList<string>();
		//RangeDataReadList<string> s2 = new RangeDataReadList<string>(s, 1, 10);
		//string v3 =s2[1];
	}

	[Test]
	public void Test1() {
		Assert.Pass();
	}


	private class TestModel : BindModelBase {
		protected override void OnPropertyChanging<T>(string? propertyName, T from, T to, CancelEventArgs cancelEventArgs) {
			base.OnPropertyChanging(propertyName, from, to, cancelEventArgs);
			if (propertyName == "") {

			}
		}
	}
}