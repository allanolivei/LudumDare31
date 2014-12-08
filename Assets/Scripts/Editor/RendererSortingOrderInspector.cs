using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomEditor(typeof(RendererSortingOrder)), CanEditMultipleObjects]
public class RendererSortingOrderInspector : Editor 
{
	
	//Armazena Sorting Layer criadas no unity
	private string[] sortingLayerNames;
	
	//Order
	private int sortingOrder;
	
	//Layer
	private int sortingLayer;
	
	//Objetos selecionados
	private Renderer[] renderer;
	
	//Se todos os objetos selecionado possuem os mesmos valores
	private bool sortingLayerEqual;
	private bool sortingOrderEqual;
	
	
	void OnEnable() 
	{
		//Cache de Sorting Layer criadas.
		sortingLayerNames = GetSortingLayerNames();
		
		//Recupera objetos selecionados
		System.Object[] objects = serializedObject.targetObjects;
		
		//Armazena valores iniciais
		Renderer first = GetAnyRenderer( objects );//(objects[0] as MonoBehaviour).renderer as Renderer;
		if( first == null ) 
		{
			renderer = new Renderer[0];
			return;
		}
		sortingOrder = first.sortingOrder;
		string layerName = first.sortingLayerName;
		sortingLayer = Mathf.Max(System.Array.IndexOf( sortingLayerNames, layerName ), 0);
		
		//Cast
		List<Renderer> rends = new List<Renderer>();
		//renderer = new Renderer[objects.Length];
		//Igualdade entre multiobjects
		sortingLayerEqual = true;
		sortingOrderEqual = true;
		for( int i = 0 ; i < objects.Length ; i++ ) 
		{
			//Cast
			//renderer[i] = (objects[i] as MonoBehaviour).renderer as Renderer;
			Renderer r = (objects[i] as MonoBehaviour).renderer as Renderer;
			if( r != null )
			{
				rends.Add( r );
				//Verifica se todos os objetos possuem o mesmo valor
				if( r.sortingOrder != sortingOrder ) sortingOrderEqual = false;
				if( r.sortingLayerName != layerName ) sortingLayerEqual = false;
			}
		}
		
		renderer = rends.ToArray();
	}
	
	protected Renderer GetAnyRenderer( System.Object[] objects )
	{
		for( int i = 0 ; i < objects.Length ; i++ ) 
		{
			Renderer r = (objects[i] as MonoBehaviour).renderer as Renderer;
			if( r != null ) return r;
		}
		return null;
	}
	
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		
		if( renderer == null || renderer.Length == 0 )
		{
			EditorGUILayout.HelpBox( "Objeto não possui componente Renderer", MessageType.Error );
			return;
		}
		
		EditorGUILayout.Space();
		
		/**
		 * SORTING Layer
		 **/
		EditorGUI.BeginChangeCheck();
		
		//UI
		EditorGUI.showMixedValue = !sortingLayerEqual;
		sortingLayer = EditorGUILayout.Popup(sortingLayer, sortingLayerNames);
		
		//Aplicar modificacoes e igualar valores
		if( EditorGUI.EndChangeCheck() ) {
			foreach( Renderer r in renderer )
			{
				r.sortingLayerName = sortingLayerNames[sortingLayer];
				EditorUtility.SetDirty(r);
			}
			sortingLayerEqual = true;
		}
		
		
		/**
		 * SORTING ORDER
		 **/
		EditorGUI.BeginChangeCheck();
		
		//UI
		EditorGUI.showMixedValue = !sortingOrderEqual;
		sortingOrder = EditorGUILayout.IntField("Order in Layer", sortingOrder);
		
		//Aplicar modificacoes e igualar valores
		if( EditorGUI.EndChangeCheck() ) 
		{
			foreach( Renderer r in renderer )
			{
				r.sortingOrder = sortingOrder;
				EditorUtility.SetDirty(r);
			}
			sortingOrderEqual = true;
		}
	}
	
	public string[] GetSortingLayerNames() 
	{
		Type t = typeof(InternalEditorUtility);
		PropertyInfo prop = t.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		return (string[])prop.GetValue(null, null);
	}
	
}