namespace RW.Common.Models;

public class ObjectWrapper<T>(T? value = default) {
	public T? Value { get; set; } = value;
}

public class StringWrapper(string? value = null) : ObjectWrapper<string?>(value);

public class IntWrapper(int value = default) : ObjectWrapper<int>(value);

public class BoolWrapper(bool value = default) : ObjectWrapper<bool>(value);

public class DoubleWrapper(double value = default) : ObjectWrapper<double>(value);

public class DateTimeWrapper(DateTime value = default) : ObjectWrapper<DateTime>(value);

public class LongWrapper(long value = default) : ObjectWrapper<long>(value);

public class FloatWrapper(float value = default) : ObjectWrapper<float>(value);

public class DecimalWrapper(decimal value = default) : ObjectWrapper<decimal>(value);

public class CharWrapper(char value = default) : ObjectWrapper<char>(value);

public class ByteWrapper(byte value = default) : ObjectWrapper<byte>(value);

public class GuidWrapper(Guid value = default) : ObjectWrapper<Guid>(value);

public class TimeSpanWrapper(TimeSpan value = default) : ObjectWrapper<TimeSpan>(value);

public class UriWrapper(Uri? value = default) : ObjectWrapper<Uri?>(value);
