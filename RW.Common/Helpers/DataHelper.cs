namespace RW.Common.Helpers;

public static class DataHelper {

	public static double[] Consecutive(int pointCount, double spacing = 1, double offset = 0) {
		double[] ys = new double[pointCount];
		for (int i = 0; i < ys.Length; i++) {
			ys[i] = i * spacing + offset;
		}
		return ys;
	}

	public static int[] ConsecutiveInt(int pointCount, int spacing = 1, int offset = 0) {
		int[] ys = new int[pointCount];
		for (int i = 0; i < ys.Length; i++) {
			ys[i] = i * spacing + offset;
		}
		return ys;
	}

}
