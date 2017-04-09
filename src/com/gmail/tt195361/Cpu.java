package com.gmail.tt195361;

public class Cpu {

	private final Registers regs;
	
	public Cpu() {
		this.regs = new Registers();
	}
	
	public void run(Memory mem) throws Casl2SimulatorException {
		for ( ; ; ) {
			int opcode = this.regs.getPr().fetch(mem);
			Instruction inst = Instruction.decode(opcode);
			inst.execute(opcode, this.regs, mem);
		}
	}
}
