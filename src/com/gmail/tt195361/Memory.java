package com.gmail.tt195361;
public class Memory {

	private final int Size = 0x10000;
	private final int MinAddress = 0x0000;
	private final int MaxAddress = 0xffff;
	
	private final int[] contents;
	
	public Memory() {
		this.contents = new int[Size];
		
		for (int index = 0; index < Size; ++index) {
			this.contents[index] = 0;
		}
	}
	
	public int read(int address) {
		// address の範囲をチェックする
		
		return this.contents[address];
	}
	
	public void write(int address, int value) {
		// address の範囲をチェックする
		
		this.contents[address] = value;
	}
	
	public void writeRange(int address, int[] values) {
		for (int index = 0; index < values.length; ++index) {
			this.contents[address] = values[index];
			++address;
		}
	}
}
