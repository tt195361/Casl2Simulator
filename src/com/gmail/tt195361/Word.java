package com.gmail.tt195361;

public class Word {

	public static int getUpperByte(int word) {
		int upperByte = BitUtils.getBits(word, 15, 8);
		return upperByte;
	}
}
