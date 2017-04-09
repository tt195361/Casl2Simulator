
public class GeneralRegister {

	private int value;
	
	public GeneralRegister() {
		reset();
	}
	
	public void reset() {
		this.value = 0;
	}

	public int getValue() {
		return this.value;
	}
	
	public void setValue(int value) {
		this.value = value;
	}
	
	@Override
	public String toString() {
		String str = String.format("%04x", this.value);
		return str;
	}
}
