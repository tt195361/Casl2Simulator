public class Registers {

	private final int GR_REGISTER_COUNT = 8;
	
	private final GeneralRegister[] grArray;
	private int sp;
	private final ProgramRegister pr;
	private int fr;
	
	public Registers() {
		this.grArray = makeGeneralRegisters();
		this.pr = new ProgramRegister();
		reset();
	}
	
	private GeneralRegister[] makeGeneralRegisters() {
		GeneralRegister[] grArray = new GeneralRegister[GR_REGISTER_COUNT];
		for (int index = 0; index < grArray.length; ++index) {
			grArray[index] = new GeneralRegister();
		}
		return grArray;
	}
	
	public void reset() {
		resetgrArray();
		this.sp = 0;
		pr.reset();;
		this.fr = 0;
	}
	
	private void resetgrArray() {
		for (GeneralRegister gr: grArray) {
			gr.reset();
		}
	}
	
	public GeneralRegister getGr(int number) {
		// number の範囲チェック
		
		return this.grArray[number];
	}
	
	public int getSp() {
		return this.sp;
	}
	
	public ProgramRegister getPr() {
		return this.pr;
	}
}
