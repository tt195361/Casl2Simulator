package com.gmail.tt195361;

public class BitUtils {
	public static int getBits(int value, int fromBit, int toBit) {
		int mask = makeMask(fromBit, toBit);
		int bits = (value & mask) >> toBit;
		return bits;
	}
	
	private static int makeMask(int fromBit, int toBit) {
		int maskSize = fromBit - toBit + 1;
		int maskBits = (1 << maskSize) - 1;
		int shiftedMaskBits = maskBits << toBit;
		return shiftedMaskBits;
	}
}
