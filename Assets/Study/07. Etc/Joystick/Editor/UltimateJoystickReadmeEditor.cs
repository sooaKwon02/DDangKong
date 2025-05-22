﻿/* UltimateJoystickReadmeEditor.cs */

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;

[InitializeOnLoad]
[CustomEditor( typeof( UltimateJoystickReadme ) )]
public class UltimateJoystickReadmeEditor : Editor
{
	static UltimateJoystickReadme readme;

	
	string Indent
	{
		get
		{
			return "    ";
		}
	}
	int sectionSpace = 20;
	int itemHeaderSpace = 10;
	int paragraphSpace = 5;
	GUIStyle titleStyle = new GUIStyle();
	GUIStyle sectionHeaderStyle = new GUIStyle();
	GUIStyle itemHeaderStyle = new GUIStyle();
	GUIStyle paragraphStyle = new GUIStyle();
	GUIStyle versionStyle = new GUIStyle();

	class PageInformation
	{
		public string pageName = "";
		public Vector2 scrollPosition = Vector2.zero;
		public delegate void TargetMethod ();
		public TargetMethod targetMethod;
	}
	static PageInformation mainMenu = new PageInformation() { pageName = "Product Manual" };
	static PageInformation gettingStarted = new PageInformation() { pageName = "Getting Started" };
	static PageInformation overview = new PageInformation() { pageName = "Overview" };
	static PageInformation documentation = new PageInformation() { pageName = "Documentation" };
	static PageInformation versionHistory = new PageInformation() { pageName = "Version History" };
	static PageInformation importantChange = new PageInformation() { pageName = "Important Change" };
	static PageInformation thankYou = new PageInformation() { pageName = "Thank You!" };
	static PageInformation settings = new PageInformation() { pageName = "Settings" };
	static List<PageInformation> pageHistory = new List<PageInformation>();
	static PageInformation currentPage = new PageInformation();

	
	bool showJoystickPositioning, showJoystickSettings, showVisualOptions, showScriptReference;
	
	class DocumentationInfo
	{
		public string functionName = "";
		public AnimBool showMore = new AnimBool( false );
		public string[] parameter;
		public string returnType = "";
		public string description = "";
		public string codeExample = "";
	}
	DocumentationInfo[] PublicFunctions = new DocumentationInfo[]
	{
		new DocumentationInfo
		{
			functionName = "UpdatePositioning()",
			description = "Updates the size and positioning of the Ultimate Joystick. This function can be used to update any options that may have been changed prior to Start().",
			codeExample = "joystick.joystickSize = 4.0f;\njoystick.UpdatePositioning();"
		},
		new DocumentationInfo()
		{
			functionName = "GetHorizontalAxis()",
			showMore = new AnimBool( false ),
			returnType = "float",
			description = "Returns the current horizontal value of the joystick's position. The value returned will always be between -1 and 1, with 0 being the neutral position.",
			codeExample = "float h = joystick.GetHorizontalAxis();"
		},
		new DocumentationInfo()
		{
			functionName = "GetVerticalAxis()",
			showMore = new AnimBool( false ),
			returnType = "float",
			description = "Returns the current vertical value of the joystick's position. The value returned will always be between -1 and 1, with 0 being the neutral position.",
			codeExample = "float v = joystick.GetVerticalAxis();"
		},
		new DocumentationInfo()
		{
			functionName = "GetHorizontalAxisRaw()",
			showMore = new AnimBool( false ),
			returnType = "float",
			description = "Returns a value of -1, 0 or 1 representing the raw horizontal value of the Ultimate Joystick.",
			codeExample = "float h = joystick.GetHorizontalAxisRaw();"
		},
		new DocumentationInfo()
		{
			functionName = "GetVerticalAxisRaw()",
			showMore = new AnimBool( false ),
			returnType = "float",
			description = "Returns a value of -1, 0 or 1 representing the raw vertical value of the Ultimate Joystick.",
			codeExample = "float v = joystick.GetVerticalAxisRaw();"
		},
		new DocumentationInfo()
		{
			functionName = "HorizontalAxis",
			showMore = new AnimBool( false ),
			returnType = "float",
			description = "Returns the current horizontal value of the joystick's position. This is a float variable that can be referenced from Game Making Plugins because it is an exposed variable.",
		},
		new DocumentationInfo()
		{
			functionName = "VerticalAxis",
			showMore = new AnimBool( false ),
			returnType = "float",
			description = "Returns the current vertical value of the joystick's position. This is a float variable that can be referenced from Game Making Plugins because it is an exposed variable.",
		},
		new DocumentationInfo()
		{
			functionName = "GetDistance()",
			showMore = new AnimBool( false ),
			returnType = "float",
			description = "Returns the distance of the joystick from it's center in a float value. The value returned will always be a value between 0 and 1.",
			codeExample = "float dist = joystick.GetDistance();"
		},
		new DocumentationInfo()
		{
			functionName = "UpdateHighlightColor()",
			showMore = new AnimBool( false ),
			parameter = new string[ 1 ]
			{
				"Color targetColor - The color to apply to the highlight images."
			},
			description = "Updates the colors of the assigned highlight images with the targeted color if the showHighlight variable is set to true. The targetColor variable will overwrite the current color setting for highlightColor and apply immediately to the highlight images.",
			codeExample = "joystick.UpdateHighlightColor( Color.white );"
		},
		new DocumentationInfo()
		{
			functionName = "UpdateTensionColors()",
			showMore = new AnimBool( false ),
			parameter = new string[ 2 ]
			{
				"Color targetTensionNone - The color to apply to the default state of the tension accent image.",
				"Color targetTensionFull - The colors to apply to the touched state of the tension accent images."
			},
			description = "Updates the tension accent image colors with the targeted colors if the showTension variable is true. The tension colors will be set to the targeted colors, and will be applied when the joystick is next used.",
			codeExample = "joystick.UpdateTensionColors( Color.white, Color.green );"
		},
		new DocumentationInfo()
		{
			functionName = "GetJoystickState()",
			showMore = new AnimBool( false ),
			returnType = "bool",
			description = "Returns the state that the joystick is currently in. This function will return true when the joystick is being interacted with, and false when not.",
			codeExample = "if( joystick.GetJoystickState() )\n{\n    Debug.Log( \"The user is interacting with the Ultimate Joystick!\" );\n}"
		},
		new DocumentationInfo()
		{
			functionName = "GetTapCount()",
			showMore = new AnimBool( false ),
			returnType = "bool",
			description = "Returns the current state of the joystick's Tap Count according to the options set. The boolean returned will be true only after the Tap Count options have been achieved from the users input.",
			codeExample = "if( joystick.GetTapCount() )\n{\n    Debug.Log( \"The user has achieved the Tap Count!\" );\n}"
		},
		new DocumentationInfo()
		{
			functionName = "DisableJoystick()",
			showMore = new AnimBool( false ),
			description = "This function will reset the Ultimate Joystick and disable the gameObject. Use this function when wanting to disable the Ultimate Joystick from being used.",
			codeExample = "joystick.DisableJoystick();"
		},
		new DocumentationInfo()
		{
			functionName = "EnableJoystick()",
			showMore = new AnimBool( false ),
			description = "This function will ensure that the Ultimate Joystick is completely reset before enabling itself to be used again.",
			codeExample = "joystick.EnableJoystick();"
		},
	};
	DocumentationInfo[] StaticFunctions = new DocumentationInfo[]
	{
		new DocumentationInfo()
		{
			functionName = "GetUltimateJoystick()",
			showMore = new AnimBool( false ),
			parameter = new string[ 1 ]
			{
				"string joystickName - The name that the targeted Ultimate Joystick has been registered with."
			},
			returnType = "UltimateJoystick",
			description = "Returns the Ultimate Joystick component that has been registered with the targeted joystick name.",
			codeExample = "UltimateJoystick moveJoystick = UltimateJoystick.GetUltimateJoystick( \"Movement\" );"
		},
		new DocumentationInfo()
		{
			functionName = "GetHorizontalAxis()",
			showMore = new AnimBool( false ),
			parameter = new string[ 1 ]
			{
				"string joystickName - The name that the targeted Ultimate Joystick has been registered with."
			},
			returnType = "float",
			description = "Returns the current horizontal value of the targeted joystick's position. The value returned will always be between -1 and 1, with 0 being the neutral position.",
			codeExample = "float h = UltimateJoystick.GetHorizontalAxis( \"Movement\" );"
		},
		new DocumentationInfo()
		{
			functionName = "GetVerticalAxis()",
			showMore = new AnimBool( false ),
			parameter = new string[ 1 ]
			{
				"string joystickName - The name that the targeted Ultimate Joystick has been registered with."
			},
			returnType = "float",
			description = "Returns the current vertical value of the targeted joystick's position. The value returned will always be between -1 and 1, with 0 being the neutral position.",
			codeExample = "float v = UltimateJoystick.GetVerticalAxis( \"Movement\" );"
		},
		new DocumentationInfo()
		{
			functionName = "GetHorizontalAxisRaw()",
			showMore = new AnimBool( false ),
			parameter = new string[ 1 ]
			{
				"string joystickName - The name that the targeted Ultimate Joystick has been registered with."
			},
			returnType = "float",
			description = "Returns a value of -1, 0 or 1 representing the raw horizontal value of the targeted Ultimate Joystick.",
			codeExample = "float h = UltimateJoystick.GetHorizontalAxisRaw( \"Movement\" );"
		},
		new DocumentationInfo()
		{
			functionName = "GetVerticalAxisRaw()",
			showMore = new AnimBool( false ),
			parameter = new string[ 1 ]
			{
				"string joystickName - The name that the targeted Ultimate Joystick has been registered with."
			},
			returnType = "float",
			description = "Returns a value of -1, 0 or 1 representing the raw vertical value of the targeted Ultimate Joystick.",
			codeExample = "float v = UltimateJoystick.GetVerticalAxisRaw( \"Movement\" );"
		},
		new DocumentationInfo()
		{
			functionName = "GetDistance()",
			showMore = new AnimBool( false ),
			parameter = new string[ 1 ]
			{
				"string joystickName - The name that the targeted Ultimate Joystick has been registered with."
			},
			returnType = "float",
			description = "Returns the distance of the joystick from it's center in a float value. This static function will return the same value as the local GetDistance function.",
			codeExample = "float dist = UltimateJoystick.GetDistance( \"Movement\" );"
		},
		new DocumentationInfo()
		{
			functionName = "GetJoystickState()",
			showMore = new AnimBool( false ),
			parameter = new string[ 1 ]
			{
				"string joystickName - The name that the targeted Ultimate Joystick has been registered with."
			},
			returnType = "bool",
			description = "Returns the state that the joystick is currently in. This function will return true when the joystick is being interacted with, and false when not.",
			codeExample = "if( UltimateJoystick.GetJoystickState( \"Movement\" ) )\n{\n    Debug.Log( \"The user is interacting with the Ultimate Joystick!\" );\n}"
		},
		new DocumentationInfo()
		{
			functionName = "GetTapCount()",
			showMore = new AnimBool( false ),
			parameter = new string[ 1 ]
			{
				"string joystickName - The name that the targeted Ultimate Joystick has been registered with."
			},
			returnType = "bool",
			description = "Returns the current state of the joystick's Tap Count according to the options set. The boolean returned will be true only after the Tap Count options have been achieved from the users input.",
			codeExample = "if( UltimateJoystick.GetTapCount( \"Movement\" ) )\n{\n    Debug.Log( \"The user has achieved the Tap Count!\" );\n}"
		},
		new DocumentationInfo()
		{
			functionName = "DisableJoystick()",
			showMore = new AnimBool( false ),
			parameter = new string[ 1 ]
			{
				"string joystickName - The name that the targeted Ultimate Joystick has been registered with."
			},
			description = "This function will reset the Ultimate Joystick and disable the gameObject. Use this function when wanting to disable the Ultimate Joystick from being used.",
			codeExample = "UltimateJoystick.DisableJoystick( \"Movement\" );"
		},
		new DocumentationInfo()
		{
			functionName = "EnableJoystick()",
			showMore = new AnimBool( false ),
			parameter = new string[ 1 ]
			{
				"string joystickName - The name that the targeted Ultimate Joystick has been registered with."
			},
			description = "This function will ensure that the Ultimate Joystick is completely reset before enabling itself to be used again.",
			codeExample = "UltimateJoystick.EnableJoystick( \"Movement\" );"
		},
	};
	DocumentationInfo[] PublicEvents = new DocumentationInfo[]
	{
		
		new DocumentationInfo()
		{
			functionName = "OnPointerDownCallback",
			description = "This event is called when the input has been pressed down on the Ultimate Joystick.",
			codeExample = "joystick.OnPointerDownCallback += YourOnPointerDownCallback;",
		},
		
		new DocumentationInfo()
		{
			functionName = "OnPointerUpCallback",
			description = "This event is called when the input has been released off the Ultimate Joystick.",
			codeExample = "joystick.OnPointerUpCallback += YourOnPointerUpCallback;",
		},
		
		new DocumentationInfo()
		{
			functionName = "OnDragCallback",
			description = "This callback will be called when the input has moved on the Ultimate Joystick.",
			codeExample = "joystick.OnDragCallback += YourOnDragCallback;",
		},
		
		new DocumentationInfo()
		{
			functionName = "OnUpdatePositioning",
			description = "This callback will be called when the Ultimate Joystick calculates it's positioning on the screen.",
			codeExample = "joystick.OnUpdatePositioning += YourOnUpdatePositioning;",
		},
	};

	class EndPageComment
	{
		public string comment = "";
		public string url = "";
	}
	EndPageComment[] endPageComments = new EndPageComment[]
	{
		new EndPageComment()
		{
			comment = "Enjoying the Ultimate Joystick? Leave us a review on the <b><color=blue>Unity Asset Store</color></b>!",
			url = "https://assetstore.unity.com/packages/slug/27695"
		},
		new EndPageComment()
		{
			comment = "Looking for a button to match the Ultimate Joystick? Check out the <b><color=blue>Ultimate Button</color></b>!",
			url = "https://www.tankandhealerstudio.com/ultimate-button.html"
		},
		new EndPageComment()
		{
			comment = "Looking for a radial menu for your game? Check out the <b><color=blue>Ultimate Radial Menu</color></b>!",
			url = "https://www.tankandhealerstudio.com/ultimate-radial-menu.html"
		},
		new EndPageComment()
		{
			comment = "Looking for a health bar for your game? Check out the <b><color=blue>Simple Health Bar FREE</color></b>!",
			url = "https://www.tankandhealerstudio.com/simple-health-bar-free.html"
		},
		new EndPageComment()
		{
			comment = "Check out our <b><color=blue>other products</color></b>!",
			url = "https://www.tankandhealerstudio.com/assets.html"
		},
	};
	int randomComment = 0;


	static UltimateJoystickReadmeEditor ()
	{
		EditorApplication.update += WaitForCompile;
	}

	static void WaitForCompile ()
	{
		if( EditorApplication.isCompiling )
			return;

		EditorApplication.update -= WaitForCompile;

		bool showReadmeObject = false;
		
		
		if( EditorPrefs.HasKey( "UltimateJoystickStartup" ) && !EditorPrefs.HasKey( "UltimateJoystickVersion" ) )
		{
			
			EditorPrefs.SetInt( "UltimateJoystickVersion", 0 );
		}

		
		if( !EditorPrefs.HasKey( "UltimateJoystickVersion" ) )
		{
			
			NavigateForward( thankYou );

			
			EditorPrefs.SetInt( "UltimateJoystickVersion", UltimateJoystickReadme.ImportantChange );

			showReadmeObject = true;
		}
		else if( EditorPrefs.GetInt( "UltimateJoystickVersion" ) < UltimateJoystickReadme.ImportantChange )
		{
			
			NavigateForward( importantChange );

			
			EditorPrefs.SetInt( "UltimateJoystickVersion", UltimateJoystickReadme.ImportantChange );

			showReadmeObject = true;
		}

		var ids = AssetDatabase.FindAssets( "README t:UltimateJoystickReadme" );
		if( ids.Length == 1 )
		{
			var readmeObject = AssetDatabase.LoadMainAssetAtPath( AssetDatabase.GUIDToAssetPath( ids[ 0 ] ) );
			readme = ( UltimateJoystickReadme )readmeObject;

			if( showReadmeObject )
				Selection.objects = new Object[] { readmeObject };
		}
	}

	void OnEnable ()
	{
		if( !pageHistory.Contains( mainMenu ) )
			pageHistory.Insert( 0, mainMenu );

		mainMenu.targetMethod = MainPage;
		gettingStarted.targetMethod = GettingStarted;
		overview.targetMethod = Overview;
		documentation.targetMethod = Documentation;
		versionHistory.targetMethod = VersionHistory;
		importantChange.targetMethod = ImportantChange;
		thankYou.targetMethod = ThankYou;
		settings.targetMethod = Settings;

		if( pageHistory.Count == 1 )
			currentPage = mainMenu;

		randomComment = Random.Range( 0, endPageComments.Length );

		readme = ( UltimateJoystickReadme )target;

		if( !EditorPrefs.HasKey( "UJ_ColorHexSetup" ) )
		{
			EditorPrefs.SetBool( "UJ_ColorHexSetup", true );
			ResetColors();
		}

		ColorUtility.TryParseHtmlString( EditorPrefs.GetString( "UJ_ColorDefaultHex" ), out readme.colorDefault );
		ColorUtility.TryParseHtmlString( EditorPrefs.GetString( "UJ_ColorValueChangedHex" ), out readme.colorValueChanged );

		Undo.undoRedoPerformed += UndoRedoCallback;
	}

	void OnDisable ()
	{
		Undo.undoRedoPerformed -= UndoRedoCallback;
	}

	void UndoRedoCallback ()
	{
		if( currentPage != settings )
			return;

		EditorPrefs.SetString( "UJ_ColorDefaultHex", "#" + ColorUtility.ToHtmlStringRGBA( readme.colorDefault ) );
		EditorPrefs.SetString( "UJ_ColorValueChangedHex", "#" + ColorUtility.ToHtmlStringRGBA( readme.colorValueChanged ) );
	}

	protected override void OnHeaderGUI ()
	{
		UltimateJoystickReadme readme = ( UltimateJoystickReadme )target;

		var iconWidth = Mathf.Min( EditorGUIUtility.currentViewWidth, 350f );

		Vector2 ratio = new Vector2( readme.icon.width, readme.icon.height ) / ( readme.icon.width > readme.icon.height ? readme.icon.width : readme.icon.height );

		GUILayout.BeginHorizontal( "In BigTitle" );
		{
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.Label( readme.icon, GUILayout.Width( iconWidth * ratio.x ), GUILayout.Height( iconWidth * ratio.y ) );
			GUILayout.Space( -20 );
			if( GUILayout.Button( readme.versionHistory[ 0 ].versionNumber, versionStyle ) && !pageHistory.Contains( versionHistory ) )
				NavigateForward( versionHistory );
			var rect = GUILayoutUtility.GetLastRect();
			if( currentPage != versionHistory )
				EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndHorizontal();
	}

	public override void OnInspectorGUI ()
	{
		paragraphStyle = new GUIStyle( EditorStyles.label ) { wordWrap = true, richText = true, fontSize = 12 };
		itemHeaderStyle = new GUIStyle( paragraphStyle ) { fontSize = 12, fontStyle = FontStyle.Bold };
		sectionHeaderStyle = new GUIStyle( paragraphStyle ) { fontSize = 14, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
		titleStyle = new GUIStyle( paragraphStyle ) { fontSize = 16, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
		versionStyle = new GUIStyle( paragraphStyle ) { alignment = TextAnchor.MiddleCenter, fontSize = 10 };

		
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( readme.settings, versionStyle, GUILayout.Width( 24 ), GUILayout.Height( 24 ) ) && !pageHistory.Contains( settings ) )
			NavigateForward( settings );
		var rect = GUILayoutUtility.GetLastRect();
		if( currentPage != settings )
			EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
		GUILayout.EndHorizontal();
		GUILayout.Space( -24 );
		GUILayout.EndVertical();

		
		EditorGUILayout.BeginHorizontal();
		EditorGUI.BeginDisabledGroup( pageHistory.Count <= 1 );
		if( GUILayout.Button( "◄", titleStyle, GUILayout.Width( 24 ) ) )
			NavigateBack();
		if( pageHistory.Count > 1 )
		{
			rect = GUILayoutUtility.GetLastRect();
			EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
		}
		EditorGUI.EndDisabledGroup();
		GUILayout.Space( -24 );

		
		GUILayout.FlexibleSpace();
		EditorGUILayout.LabelField( currentPage.pageName, titleStyle );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		
		if( currentPage.targetMethod != null )
			currentPage.targetMethod();

		Repaint();
	}

	void StartPage ( PageInformation pageInfo )
	{
		pageInfo.scrollPosition = EditorGUILayout.BeginScrollView( pageInfo.scrollPosition, false, false );
		GUILayout.Space( 15 );
	}

	void EndPage ()
	{
		EditorGUILayout.EndScrollView();
	}

	static void NavigateBack ()
	{
		pageHistory.RemoveAt( pageHistory.Count - 1 );
		currentPage = pageHistory[ pageHistory.Count - 1 ];
	}

	static void NavigateForward ( PageInformation menu )
	{
		pageHistory.Add( menu );
		currentPage = menu;
	}

	void MainPage ()
	{
		StartPage( mainMenu );

		EditorGUILayout.LabelField( "We hope that you are enjoying using the Ultimate Joystick in your project!", paragraphStyle );
		EditorGUILayout.Space();
		EditorGUILayout.LabelField( "As with any package, you may be having some trouble understanding how to get the Ultimate Joystick working in your project. If so, have no fear, Tank & Healer Studio is here! Here is a few things that can help you get started:", paragraphStyle );

		EditorGUILayout.Space();

		EditorGUILayout.LabelField( "  • Read the <b><color=blue>Getting Started</color></b> section of this README!", paragraphStyle );
		var rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
		if( Event.current.type == EventType.MouseDown && rect.Contains( Event.current.mousePosition ) )
			NavigateForward( gettingStarted );

		EditorGUILayout.Space();

		EditorGUILayout.LabelField( "  • To learn more about the options on the inspector, read the <b><color=blue>Overview</color></b> section!", paragraphStyle );
		rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
		if( Event.current.type == EventType.MouseDown && rect.Contains( Event.current.mousePosition ) )
			NavigateForward( overview );

		EditorGUILayout.Space();

		EditorGUILayout.LabelField( "  • Check out the <b><color=blue>Documentation</color></b> section!", paragraphStyle );
		rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
		if( Event.current.type == EventType.MouseDown && rect.Contains( Event.current.mousePosition ) )
			NavigateForward( documentation );

		EditorGUILayout.Space();

		EditorGUILayout.LabelField( "  • Watch our <b><color=blue>Video Tutorials</color></b> on the Ultimate Joystick!", paragraphStyle );
		rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
		if( Event.current.type == EventType.MouseDown && rect.Contains( Event.current.mousePosition ) )
		{
			Debug.Log( "Ultimate Joystick\nOpening YouTube Tutorials" );
			Application.OpenURL( "https://www.youtube.com/playlist?list=PL7crd9xMJ9TmWdbR_bklluPeElJ_xUdO9" );
		}

		EditorGUILayout.Space();

		EditorGUILayout.LabelField( "  • <b><color=blue>Contact Us</color></b> directly with your issue! We'll try to help you out as much as we can.", paragraphStyle );
		rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
		if( Event.current.type == EventType.MouseDown && rect.Contains( Event.current.mousePosition ) )
		{
			Debug.Log( "Ultimate Joystick\nOpening Online Contact Form" );
			Application.OpenURL( "https://www.tankandhealerstudio.com/contact-us.html" );
		}

		EditorGUILayout.Space();

		EditorGUILayout.LabelField( "Now you have the tools you need to get the Ultimate Joystick working in your project. Now get out there and make your awesome game!", paragraphStyle );

		EditorGUILayout.Space();

		EditorGUILayout.LabelField( "Happy Game Making,\n" + Indent + "Tank & Healer Studio", paragraphStyle );

		EditorGUILayout.Space();

		GUILayout.FlexibleSpace();

		EditorGUILayout.LabelField( endPageComments[ randomComment ].comment, paragraphStyle );
		rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
		if( Event.current.type == EventType.MouseDown && rect.Contains( Event.current.mousePosition ) )
			Application.OpenURL( endPageComments[ randomComment ].url );

		EndPage();
	}

	void GettingStarted ()
	{
		StartPage( gettingStarted );

		EditorGUILayout.LabelField( "How To Create", sectionHeaderStyle );

		GUILayout.Space( paragraphSpace );

		EditorGUILayout.LabelField( Indent + "To create an Ultimate Joystick in your scene, simply find the Ultimate Joystick prefab that you would like to add and click and drag the prefab into the scene. The Ultimate Joystick prefab will automatically find or create a canvas in your scene for you.", paragraphStyle );

		GUILayout.Space( sectionSpace );

		EditorGUILayout.LabelField( "How To Reference", sectionHeaderStyle );

		GUILayout.Space( paragraphSpace );

		EditorGUILayout.LabelField( "   One of the great things about the Ultimate Joystick is how easy it is to reference to other scripts. The first thing that you will want to make sure to do is to name the joystick in the Script Reference section. After this is complete, you will be able to reference that particular joystick by the name that you have assigned to it.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		EditorGUILayout.LabelField( "After the joystick has been given a name in the Script Reference section, we can get that joystick's position by catching the Horizontal and Vertical values at run time and storing the results from the static functions: <i>GetHorizontalAxis</i> and <i>GetVerticalAxis</i>. These functions will return the joystick's position, and will be float values between -1, and 1, with 0 being at the center. For more information about these functions, and others that are available to the Ultimate Joystick class, please see the Documentation section of this window.", paragraphStyle );

		GUILayout.Space( sectionSpace );

		EditorGUILayout.LabelField( "Simple Example", sectionHeaderStyle );

		GUILayout.Space( paragraphSpace );

		EditorGUILayout.LabelField( "   Let's assume that we want to use a joystick for a characters movement. The first thing to do is to assign the name \"Movement\" in the Joystick Name variable located in the Script Reference section of the Ultimate Joystick.", paragraphStyle );

		Vector2 ratio = new Vector2( readme.scriptReference.width, readme.scriptReference.height ) / ( readme.scriptReference.width > readme.scriptReference.height ? readme.scriptReference.width : readme.scriptReference.height );

		float imageWidth = readme.scriptReference.width > Screen.width - 50 ? Screen.width - 50 : readme.scriptReference.width;

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label( readme.scriptReference, GUILayout.Width( imageWidth ), GUILayout.Height( imageWidth * ratio.y ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.LabelField( "After that, we need to create two float variables to store the result of the joystick's position. In order to get the \"Movement\" joystick's position, we need to pass in the name \"Movement\" as the argument.", paragraphStyle );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "Coding Example:", itemHeaderStyle );
		EditorGUILayout.TextArea( "float h = UltimateJoystick.GetHorizontalAxis( \"Movement\" );\nfloat v = UltimateJoystick.GetVerticalAxis( \"Movement\" );", GUI.skin.GetStyle( "TextArea" ) );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "The h and v variables now contain the values of the Movement joystick's position. Now we can use this information in any way that is desired. For example, if you are wanting to put the joystick's position into a character movement script, you would create a Vector3 variable for movement direction, and put in the appropriate values of this joystick's position.", paragraphStyle );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "Coding Example:", itemHeaderStyle );
		EditorGUILayout.TextArea( "Vector3 movementDirection = new Vector3( h, 0, v );", GUI.skin.GetStyle( "TextArea" ) );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "In the above example, the h variable is used to in the X slot of the Vector3, and the v variable is in the Z slot. This is because you generally don't want your character to move in the Y direction unless the user jumps. That is why we put the v variable into the Z value of the movementDirection variable.", paragraphStyle );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "Understanding how to use the values from any input is important when creating character controllers, so experiment with the values and try to understand how user input can be used in different ways.", paragraphStyle );

		GUILayout.Space( itemHeaderSpace );

		EndPage();
	}

	void Overview ()
	{
		StartPage( overview );

		EditorGUILayout.LabelField( "Sections", sectionHeaderStyle );
		EditorGUILayout.LabelField( Indent + "The display below is mimicking the Ultimate Joystick inspector. Expand each section to learn what each one is designed for.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		
		GUIStyle toolbarStyle = new GUIStyle( EditorStyles.toolbarButton ) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 11 };
		
		showJoystickPositioning = GUILayout.Toggle( showJoystickPositioning, ( showJoystickPositioning ? "▼" : "►" ) + "Joystick Positioning", toolbarStyle );
		if( showJoystickPositioning )
		{
			GUILayout.Space( paragraphSpace );
			EditorGUILayout.LabelField( "This section handles the positioning of the Ultimate Joystick on the screen.", paragraphStyle );
		}

		EditorGUILayout.Space();

		
		showJoystickSettings = GUILayout.Toggle( showJoystickSettings, ( showJoystickSettings ? "▼" : "►" ) + "Joystick Settings", toolbarStyle );
		if( showJoystickSettings )
		{
			GUILayout.Space( paragraphSpace );
			EditorGUILayout.LabelField( "This section contains the various settings for how the joystick will feel to the player.", paragraphStyle );
		}

		EditorGUILayout.Space();

		
		showVisualOptions = GUILayout.Toggle( showVisualOptions, ( showVisualOptions ? "▼" : "►" ) + "Visual Options", toolbarStyle );
		if( showVisualOptions )
		{
			GUILayout.Space( paragraphSpace );

			EditorGUILayout.LabelField( "The options in this section give the joystick a visual boost. The options in this section don't have an effect on joystick functionality. They are designed to enhance the visual display.", paragraphStyle );
		}

		EditorGUILayout.Space();

		
		showScriptReference = GUILayout.Toggle( showScriptReference, ( showScriptReference ? "▼" : "►" ) + "Script Reference", toolbarStyle );
		if( showScriptReference )
		{
			GUILayout.Space( paragraphSpace );
			EditorGUILayout.LabelField( "In this section you will be able to setup the reference to this Ultimate Joystick, and you will be provided with code examples to be able to copy and paste into your own scripts.", paragraphStyle );
		}

		GUILayout.Space( sectionSpace );

		EditorGUILayout.LabelField( "Tooltips", sectionHeaderStyle );
		EditorGUILayout.LabelField( Indent + "To learn more about each option in these sections, please select the Ultimate Joystick in your scene, and hover over each item to read the provided tooltip.", paragraphStyle );

		EndPage();


	}

	void Documentation ()
	{
		StartPage( documentation );
		
		EditorGUILayout.LabelField( "Static Functions", sectionHeaderStyle );

		GUILayout.Space( paragraphSpace );

		EditorGUILayout.LabelField( Indent + "The following functions can be referenced from your scripts without the need for an assigned local Ultimate Joystick variable. However, each function must have the targeted Ultimate Joystick name in order to find the correct Ultimate Joystick in the scene. Each example code provided uses the name 'Movement' as the joystick name.", paragraphStyle );

		Vector2 ratio = new Vector2( readme.scriptReference.width, readme.scriptReference.height ) / ( readme.scriptReference.width > readme.scriptReference.height ? readme.scriptReference.width : readme.scriptReference.height );

		float imageWidth = readme.scriptReference.width > Screen.width - 50 ? Screen.width - 50 : readme.scriptReference.width;

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label( readme.scriptReference, GUILayout.Width( imageWidth ), GUILayout.Height( imageWidth * ratio.y ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.LabelField( "Please click on the function name to learn more.", paragraphStyle );

		for( int i = 0; i < StaticFunctions.Length; i++ )
			ShowDocumentation( StaticFunctions[ i ] );

		GUILayout.Space( sectionSpace );
		
		EditorGUILayout.LabelField( "Public Functions", sectionHeaderStyle );

		GUILayout.Space( paragraphSpace );

		EditorGUILayout.LabelField( Indent + "All of the following public functions are only available from a reference to the Ultimate Joystick. Each example provided relies on having a Ultimate Joystick variable named 'joystick' stored inside your script. When using any of the example code provided, make sure that you have a public Ultimate Joystick variable like the one below:", paragraphStyle );

		EditorGUILayout.TextArea( "public UltimateJoystick joystick;", GUI.skin.textArea );

		GUILayout.Space( paragraphSpace );

		for( int i = 0; i < PublicFunctions.Length; i++ )
			ShowDocumentation( PublicFunctions[ i ] );

		GUILayout.Space( sectionSpace );

		
		EditorGUILayout.LabelField( "Events", sectionHeaderStyle );

		GUILayout.Space( paragraphSpace );

		EditorGUILayout.LabelField( Indent + "These events are called when certain actions are performed on the Ultimate Joystick. By subscribing a function to these events you will be notified with the particular action is performed.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		for( int i = 0; i < PublicEvents.Length; i++ )
			ShowDocumentation( PublicEvents[ i ] );

		GUILayout.Space( itemHeaderSpace );

		EndPage();
	}

	void VersionHistory ()
	{
		StartPage( versionHistory );

		for( int i = 0; i < readme.versionHistory.Length; i++ )
		{
			EditorGUILayout.LabelField( "Version " + readme.versionHistory[ i ].versionNumber, itemHeaderStyle );

			for( int n = 0; n < readme.versionHistory[ i ].changes.Length; n++ )
				EditorGUILayout.LabelField( "  • " + readme.versionHistory[ i ].changes[ n ] + ".", paragraphStyle );

			if( i < ( readme.versionHistory.Length - 1 ) )
				GUILayout.Space( itemHeaderSpace );
		}
		
		EndPage();
	}

	void ImportantChange ()
	{
		StartPage( importantChange );

		EditorGUILayout.LabelField( Indent + "Thank you for downloading the most recent version of the Ultimate Joystick. If you are experiencing any errors, please completely remove the Ultimate Joystick from your project and re-import it. As always, if you run into any issues with the Ultimate Joystick, please contact us at:", paragraphStyle );

		GUILayout.Space( paragraphSpace );
		EditorGUILayout.SelectableLabel( "tankandhealerstudio@outlook.com", itemHeaderStyle, GUILayout.Height( 15 ) );
		GUILayout.Space( sectionSpace );

		EditorGUILayout.LabelField( "INTERNAL CHANGES", sectionHeaderStyle );
		EditorGUILayout.LabelField( "  There were quite a few internal changes that happened in version 3.0.0, so some of your joysticks may behave a little strange at first. All that should be needed to fix them is to select the Ultimate Joystick game object in your hierarchy. Once you have done this the editor script can fix the old joysticks that are using depreciated variables.", paragraphStyle );

		GUILayout.Space( itemHeaderSpace );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Got it!", GUILayout.Width( Screen.width / 2 ) ) )
			NavigateBack();

		var rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );

		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EndPage();
	}

	void ThankYou ()
	{
		StartPage( thankYou );

		EditorGUILayout.LabelField( "The two of us at Tank & Healer Studio would like to thank you for purchasing the Ultimate Joystick asset package from the Unity Asset Store.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		EditorGUILayout.LabelField( "We hope that the Ultimate Joystick will be a great help to you in the development of your game. Here is a few things that can help you get started:", paragraphStyle );

		EditorGUILayout.Space();

		EditorGUILayout.LabelField( "  • Read the <b><color=blue>Getting Started</color></b> section of this README!", paragraphStyle );
		var rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
		if( Event.current.type == EventType.MouseDown && rect.Contains( Event.current.mousePosition ) )
			NavigateForward( gettingStarted );

		EditorGUILayout.Space();

		EditorGUILayout.LabelField( "  • To learn more about the options on the inspector, read the <b><color=blue>Overview</color></b> section!", paragraphStyle );
		rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
		if( Event.current.type == EventType.MouseDown && rect.Contains( Event.current.mousePosition ) )
			NavigateForward( overview );

		EditorGUILayout.Space();

		EditorGUILayout.LabelField( "  • Check out the <b><color=blue>Documentation</color></b> section to learn more about how to use the Ultimate Joystick in your scripts!", paragraphStyle );
		rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
		if( Event.current.type == EventType.MouseDown && rect.Contains( Event.current.mousePosition ) )
			NavigateForward( documentation );

		EditorGUILayout.Space();

		EditorGUILayout.LabelField( "You can access this information at any time by clicking on the <b>README</b> file inside the Ultimate Joystick folder.", paragraphStyle );

		EditorGUILayout.Space();

		EditorGUILayout.LabelField( "Again, thank you for downloading the Ultimate Joystick. We hope that your project is a success!", paragraphStyle );

		EditorGUILayout.Space();

		EditorGUILayout.LabelField( "Happy Game Making,\n" + Indent + "Tank & Healer Studio", paragraphStyle );

		GUILayout.Space( 15 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Continue", GUILayout.Width( Screen.width / 2 ) ) )
			NavigateBack();

		var rect2 = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect2, MouseCursor.Link );

		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EndPage();
	}
	
	void Settings ()
	{
		StartPage( settings );

		EditorGUILayout.LabelField( "Gizmo Colors", sectionHeaderStyle );
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField( serializedObject.FindProperty( "colorDefault" ), new GUIContent( "Default" ) );
		if( EditorGUI.EndChangeCheck() )
		{
			serializedObject.ApplyModifiedProperties();
			EditorPrefs.SetString( "UJ_ColorDefaultHex", "#" + ColorUtility.ToHtmlStringRGBA( readme.colorDefault ) );
		}

		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField( serializedObject.FindProperty( "colorValueChanged" ), new GUIContent( "Value Changed" ) );
		if( EditorGUI.EndChangeCheck() )
		{
			serializedObject.ApplyModifiedProperties();
			EditorPrefs.SetString( "UJ_ColorValueChangedHex", "#" + ColorUtility.ToHtmlStringRGBA( readme.colorValueChanged ) );
		}

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Reset", GUILayout.Width( EditorGUIUtility.currentViewWidth / 2 ) ) )
		{
			if( EditorUtility.DisplayDialog( "Reset Gizmo Color", "Are you sure that you want to reset the gizmo colors back to default?", "Yes", "No" ) )
				ResetColors();
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		if( EditorPrefs.GetBool( "UUI_DevelopmentMode" ) )
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField( "Development Mode", sectionHeaderStyle );
			base.OnInspectorGUI();
			EditorGUILayout.Space();
		}

		GUILayout.FlexibleSpace();

		GUILayout.Space( sectionSpace );

		EditorGUI.BeginChangeCheck();
		GUILayout.Toggle( EditorPrefs.GetBool( "UUI_DevelopmentMode" ), ( EditorPrefs.GetBool( "UUI_DevelopmentMode" ) ? "Disable" : "Enable" ) + " Development Mode", EditorStyles.radioButton );
		if( EditorGUI.EndChangeCheck() )
		{
			if( EditorPrefs.GetBool( "UUI_DevelopmentMode" ) == false )
			{
				if( EditorUtility.DisplayDialog( "Enable Development Mode", "Are you sure you want to enable development mode for the Ultimate Joystick? This mode will allow you to see the default inspector for the Ultimate Joystick which is useful when adding variables to this script without having to edit the custom editor script.", "Enable", "Cancel" ) )
				{
					EditorPrefs.SetBool( "UUI_DevelopmentMode", !EditorPrefs.GetBool( "UUI_DevelopmentMode" ) );
				}
			}
			else
				EditorPrefs.SetBool( "UUI_DevelopmentMode", !EditorPrefs.GetBool( "UUI_DevelopmentMode" ) );
		}

		EndPage();
	}

	void ResetColors ()
	{
		serializedObject.FindProperty( "colorDefault" ).colorValue = new Color( 1.0f, 1.0f, 1.0f, 0.5f );
		serializedObject.FindProperty( "colorValueChanged" ).colorValue = new Color( 0.0f, 1.0f, 0.0f, 1.0f );
		serializedObject.ApplyModifiedProperties();

		EditorPrefs.SetString( "UJ_ColorDefaultHex", "#" + ColorUtility.ToHtmlStringRGBA( new Color( 1.0f, 1.0f, 1.0f, 0.5f ) ) );
		EditorPrefs.SetString( "UJ_ColorValueChangedHex", "#" + ColorUtility.ToHtmlStringRGBA( new Color( 0.0f, 1.0f, 0.0f, 1.0f ) ) );
	}

	void ShowDocumentation ( DocumentationInfo info )
	{
		GUILayout.Space( paragraphSpace );

		EditorGUILayout.LabelField( info.functionName, itemHeaderStyle );
		var rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
		if( Event.current.type == EventType.MouseDown && rect.Contains( Event.current.mousePosition ) && ( info.showMore.faded == 0.0f || info.showMore.faded == 1.0f ) )
		{
			info.showMore.target = !info.showMore.target;
			GUI.FocusControl( "" );
		}

		if( EditorGUILayout.BeginFadeGroup( info.showMore.faded ) )
		{
			EditorGUILayout.LabelField( Indent + "<i>Description:</i> " + info.description, paragraphStyle );

			if( info.parameter != null )
			{
				for( int i = 0; i < info.parameter.Length; i++ )
					EditorGUILayout.LabelField( Indent + "<i>Parameter:</i> " + info.parameter[ i ], paragraphStyle );
			}
			if( info.returnType != string.Empty )
				EditorGUILayout.LabelField( Indent + "<i>Return type:</i> " + info.returnType, paragraphStyle );

			if( info.codeExample != string.Empty )
				EditorGUILayout.TextArea( info.codeExample, GUI.skin.textArea );

			GUILayout.Space( paragraphSpace );
		}
		EditorGUILayout.EndFadeGroup();
	}

	public static void OpenReadmeDocumentation ()
	{
		var ids = AssetDatabase.FindAssets( "README t:UltimateJoystickReadme" );
		if( ids.Length == 1 )
		{
			var readmeObject = AssetDatabase.LoadMainAssetAtPath( AssetDatabase.GUIDToAssetPath( ids[ 0 ] ) );
			Selection.objects = new Object[] { readmeObject };
			readme = ( UltimateJoystickReadme )readmeObject;

			if( !pageHistory.Contains( documentation ) )
				NavigateForward( documentation );
		}
		else
		{
			Debug.LogError( "There is no README object in the Ultimate Joystick folder." );
		}
	}
}