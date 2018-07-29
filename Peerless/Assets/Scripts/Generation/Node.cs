using System;

// For the purposes of Peerless, <T> will be a 4-length array: [x-position, y-position, width, height].
public class Node<T>{
	public T value;
	public int order = 0;				// For self-balancing reasons... I hope.
	public Node<T> leftChild;
	public Node<T> rightChild;

	public Node(){
	}
	public Node(T data){
		this.value = data;
	}

	public T GetData(){
		return this.value;
	}
	public void SetData(T data){
		value = data;
	}
}
