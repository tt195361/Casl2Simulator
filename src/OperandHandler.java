
public class OperandHandler {
	public static int readAddr(Registers regs, Memory mem) {
		 return regs.getPr().fetch(mem);
	}
	
	public static int readAddrContents(int opcode, Registers regs, Memory mem) {
		int addr = readAddr(regs, mem);
		// TODO: opcode を使って x を指定できるようにする。
		int contents = mem.read(addr);
		return contents;
	}
	
	public static GeneralRegister getR1(int opcode, Registers regs) {
		int number = BitUtils.getBits(opcode, 6, 4);
		GeneralRegister gr = regs.getGr(number);
		return gr;
	}
}
