package com.gmail.tt195361;

public class Main {

	public static void main(String[] args) {
		Memory mem = new Memory();
		SetupMemory(mem);
		
		Cpu cpu = new Cpu();
		
		try {
			cpu.run(mem);
		}
		catch (Casl2SimulatorException ex) {
			String message = ex.getMessage();
			System.out.println(message);
		}
	}
	
	private static void SetupMemory(Memory mem) {
		int[] initialMemAddr0x0000 = new int[] {
			0x1030, 0x0500,		// LD  r3,0x0500
		};
		int[] initialMemAddr0x0500 = new int[] {
			0x1234,
		};
			
		mem.writeRange(0x0000, initialMemAddr0x0000);
		mem.writeRange(0x0500, initialMemAddr0x0500);
	}
}
