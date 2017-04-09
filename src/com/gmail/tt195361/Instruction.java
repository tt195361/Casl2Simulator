package com.gmail.tt195361;

public enum Instruction {

	NOP(0x00) {
		public void execute(int opcode, Registers regs, Memory mem) {
			// 何もしない。
		}
	},
	LD_raddrx(0x10) {
		public void execute(int opcode, Registers regs, Memory mem) {
			GeneralRegister r1 = OperandHandler.getR1(opcode, regs);
			int operand = OperandHandler.readAddrContents(opcode, regs, mem);
			r1.setValue(operand);
		}
	},
	ST(0x11) {
		public void execute(int opcode, Registers regs, Memory mem) {
			// 何もしない。
		}
	},
	LAD(0x12) {
		public void execute(int opcode, Registers regs, Memory mem) {
			// 何もしない。
		}
	},
	LD_r1r2(0x14) {
		public void execute(int opcode, Registers regs, Memory mem) {
			// 何もしない。
		}
	};
	
	private final int opcodeUpperByte;
	
	private Instruction(int opcodeUpperByte) {
		this.opcodeUpperByte = opcodeUpperByte;
	}
	
	public static Instruction decode(int opcode) throws Casl2SimulatorException {
		int opcodeUpperByte = Word.getUpperByte(opcode);
		for (Instruction inst: Instruction.values()) {
			if (inst.opcodeUpperByte == opcodeUpperByte) {
				return inst;
			}
		}
		
		String message = String.format("未定義の命令です: opcode=0x%02x", opcode);
		throw new Casl2SimulatorException(message);
	}
	
	public abstract void execute(int opcode, Registers regs, Memory mem);
}
