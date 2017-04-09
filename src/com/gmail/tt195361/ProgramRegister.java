package com.gmail.tt195361;

public class ProgramRegister {

	private int value;
	
	public ProgramRegister() {
		reset();
	}
	
	public void reset() {
		this.value = 0;
	}
	
	public int fetch(Memory mem) {;
		int contents = mem.read(this.value);
		++this.value;
		
		return contents;
	}
	
	@Override
	public String toString() {
		String str = String.format("%04x", this.value);
		return str;
	}
}
