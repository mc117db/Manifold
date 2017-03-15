using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TConfig  {


	private static List<TagTemplate> _tags = new List<TagTemplate>();


	public static void initTags() {
		AddTag ("TODO", "#TODO");
		AddTag ("FIX",  "#FIXME");
		AddTag ("CRITICAL",  "#CRITICAL");
		AddTag ("REFACTOR", "#REFACTOR");
		AddTag ("HACK", "#MESS");
	}



	public static List<TagTemplate> tags {
		get {
			return _tags;
		}
	}


	private static void AddTag(string patern, string name) {
		TagTemplate tpl = new TagTemplate (patern, name);
		_tags.Add (tpl);
	}
	
	
}
