package com.gmail.tt195361;

import static org.junit.Assert.*;
import org.junit.Test;

public class BitUtilsTest {

	@Test
	public void testGetBits() {
		checkGetBits(0xffff, 5, 3, 7, "0xffff ‚Ìƒrƒbƒg 5..3 => 7");
	}
	
	private void checkGetBits(
			int value, int fromBit, int toBit, int expected, String message) {
		int actual = BitUtils.getBits(value, fromBit, toBit);
		assertEquals(message, expected, actual);
	}
}
