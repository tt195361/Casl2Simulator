package com.gmail.tt195361;

import org.junit.Test;
import static org.junit.Assert.*;
import static org.hamcrest.CoreMatchers.*;

public class InstructionTest {

	@Test
	public void testDecode() {
		checkDecode(0x1000, Instruction.LD_raddrx, "0x10xx => LD r,adr[,x]");
		
		checkDecode(0xff00, null, "0xffxx => –¢’è‹`");
	}
	
	private void checkDecode(int opcode, Instruction expected, String message) {
		try {
			Instruction actual = Instruction.decode(opcode);
			assertThat(message, actual, is(expected));
		}
		catch (Casl2SimulatorException ex) {
			assertNull(expected);
		}
	}
}
