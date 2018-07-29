using System.Collections;
using UnityEngine;

public class BinaryTree{
	public Node<int[]> root;

	public BinaryTree(){
		this.root = null;
	}

	public virtual void Clear(){
		root = null;
	}

	public virtual void Insert(Node<int[]> curNode, Node<int[]> newNode){
		if (curNode == null) {
			curNode = newNode;
			return;
		}
		if (curNode.leftChild == null) {
			Insert (curNode.leftChild, newNode);
		} else if (curNode.rightChild == null) {
			Insert (curNode.rightChild, newNode);
		} else {
			// Runs only if both children exist.
			if (curNode.leftChild.order <= curNode.rightChild.order) {
				curNode.leftChild.order += 1;
				Insert (curNode.leftChild, newNode);
			} else {
				curNode.rightChild.order += 1;
				Insert (curNode.rightChild, newNode);
			}
		}

	}

	public void PreTraversal(Node<int[]> curNode){
		if (curNode == null) {
			return;
		}
		Debug.Log (curNode.value);
		PreTraversal (curNode.leftChild);
		PreTraversal (curNode.rightChild);
	}

	public void InTraversal(Node<int[]> curNode){
		if (curNode == null) {
			return;
		}
		InTraversal (curNode.leftChild);
		Debug.Log (curNode.value);
		InTraversal (curNode.rightChild);
	}

	public void PostTraversal(Node<int[]> curNode){
		if (curNode == null) {
			return;
		}
		PostTraversal (curNode.leftChild);
		PostTraversal (curNode.rightChild);
		Debug.Log (curNode.value);
	}
}
