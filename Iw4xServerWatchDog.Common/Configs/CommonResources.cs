using System;
using System.Collections.Generic;
using System.Drawing;
using Iw4xServerWatchDog.Common.Types;
using PortableJsonConfig;

// ReSharper disable StringLiteralTypo

namespace Iw4xServerWatchDog.Common.Configs
{
	public class CommonResources : IConfig<CommonResources>, ICommonResources
	{
		public string ConfigFileName { get; } = "Resources";
		public string ConfigFolderName { get; } = "Configs";

		public Dictionary<string, Map> Map { get; set; } = new Dictionary<string, Map>
		{
			["mp_afghan"] = new Map
			{
				Name  = "Afghan",
				Image = @"http://www.themodernwarfare2.com/images/mw2/maps/afghan-t.jpg"
			},
			["mp_derail"] = new Map
			{
				Name  = "Derail",
				Image = @"http://www.themodernwarfare2.com/images/mw2/maps/derail-t.jpg"
			},
			["mp_estate"] = new Map
			{
				Name  = "Estate",
				Image = @"http://www.themodernwarfare2.com/images/mw2/maps/estate-t.jpg"
			},
			["mp_favela"] = new Map
			{
				Name  = "Favela",
				Image = @"http://www.themodernwarfare2.com/images/mw2/maps/favela-t.jpg"
			},
			["mp_highrise"] = new Map
			{
				Name  = "Highrise",
				Image = @"http://www.themodernwarfare2.com/images/mw2/maps/highrise-t.jpg"
			},
			["mp_invasion"] = new Map
			{
				Name  = "Invasion",
				Image = @"http://www.themodernwarfare2.com/images/mw2/maps/invasion-t.jpg"
			},
			["mp_checkpoint"] = new Map
			{
				Name  = "Karachi",
				Image = @"http://www.themodernwarfare2.com/images/mw2/maps/karachi-t.jpg"
			},
			["mp_quarry"] = new Map
			{
				Name  = "Quarry",
				Image = @"http://www.themodernwarfare2.com/images/mw2/maps/quarry-t.jpg"
			},
			["mp_rundown"] = new Map
			{
				Name  = "Rundown",
				Image = @"http://www.themodernwarfare2.com/images/mw2/maps/rundown-t.jpg"
			},
			["mp_rust"] = new Map
			{
				Name  = "Rust",
				Image = @"http://www.themodernwarfare2.com/images/mw2/maps/rust-t.jpg"
			},
			["mp_boneyard"] = new Map
			{
				Name  = "Scrapyard",
				Image = @"http://www.themodernwarfare2.com/images/mw2/maps/scrapyard-t.jpg"
			},
			["mp_nightshift"] = new Map
			{
				Name  = "Skidrow",
				Image = @"http://www.themodernwarfare2.com/images/mw2/maps/skidrow-t.jpg"
			},
			["mp_subbase"] = new Map
			{
				Name  = "Sub Base",
				Image = @"http://www.themodernwarfare2.com/images/mw2/maps/sub-base-t.jpg"
			},
			["mp_terminal"] = new Map
			{
				Name  = "Terminal",
				Image = @"http://www.themodernwarfare2.com/images/mw2/maps/terminal-t.jpg"
			},
			["mp_underpass"] = new Map
			{
				Name  = "Underpass",
				Image = @"http://www.themodernwarfare2.com/images/mw2/maps/underpass-t.jpg"
			},
			["mp_brecourt"] = new Map
			{
				Name  = "Wasteland",
				Image = @"http://www.themodernwarfare2.com/images/mw2/maps/wasteland-t.jpg"
			},
			["mp_complex"] = new Map
			{
				Name = "Bailout",
				Image =
					@"https://vignette.wikia.nocookie.net/callofduty/images/0/0e/MW2_Bailout.jpg/revision/latest/scale-to-width-down/300?cb=20100613115812"
			},
			["mp_crash"] = new Map
			{
				Name = "Crash",
				Image =
					@"https://vignette.wikia.nocookie.net/callofduty/images/9/90/Bare_Load_Screen_Crash_CoD4.jpg/revision/latest/scale-to-width-down/300?cb=20110727174701"
			},
			["mp_overgrown"] = new Map
			{
				Name = "Overgrown",
				Image =
					@"https://vignette.wikia.nocookie.net/callofduty/images/7/7d/Bare_Load_Screen_Overgrown_CoD4.jpg/revision/latest/scale-to-width-down/300?cb=20110727174104"
			},
			["mp_compact"] = new Map
			{
				Name = "Salvage",
				Image =
					@"https://vignette.wikia.nocookie.net/callofduty/images/d/d7/MW2_Salvage.jpg/revision/latest/scale-to-width-down/300?cb=20100613115824"
			},
			["mp_storm"] = new Map
			{
				Name = "Storm",
				Image =
					@"https://vignette.wikia.nocookie.net/callofduty/images/6/60/MW2_Storm.jpg/revision/latest/scale-to-width-down/300?cb=20100613115722"
			},
			["mp_abandon"] = new Map
			{
				Name = "Carnival",
				Image =
					@"https://vignette.wikia.nocookie.net/callofduty/images/c/c3/Carnival_loadscreen.jpg/revision/latest/scale-to-width-down/300?cb=20100712195429"
			},
			["mp_fuel2"] = new Map
			{
				Name = "Fuel",
				Image =
					@"https://vignette.wikia.nocookie.net/callofduty/images/d/de/Fuel_loadscreen.jpg/revision/latest/scale-to-width-down/300?cb=20100712195521"
			},
			["mp_strike"] = new Map
			{
				Name = "Strike",
				Image =
					@"https://vignette.wikia.nocookie.net/callofduty/images/f/fe/Cod4_map_strike.jpg/revision/latest/scale-to-width-down/300?cb=20110727175654"
			},
			["mp_trailerpark"] = new Map
			{
				Name = "Trailer Park",
				Image =
					@"https://vignette.wikia.nocookie.net/callofduty/images/c/cf/Trailer_Park.jpg/revision/latest/scale-to-width-down/300?cb=20100712195448"
			},
			["mp_vacant"] = new Map
			{
				Name = "Vacant",
				Image =
					@"https://vignette.wikia.nocookie.net/callofduty/images/f/f6/Cod4_map_vacant.jpg/revision/latest/scale-to-width-down/300?cb=20100723080839"
			},
			["mp_nuked"] = new Map
			{
				Name = "Nuketown",
				Image =
					@"https://vignette.wikia.nocookie.net/callofduty/images/2/2c/Bare_Load_Screen_Nuketown_BO.jpg/revision/latest/scale-to-width-down/300?cb=20110303122337"
			},
			["mp_killhouse"] = new Map
			{
				Name = "Killhouse",
				Image =
					@"https://vignette.wikia.nocookie.net/callofduty/images/4/48/Cod4-killhouse.jpg/revision/latest/scale-to-width-down/300?cb=20100723081127"
			},
			["mp_shipment"] = new Map
			{
				Name = "Shipment",
				Image =
					@"https://vignette.wikia.nocookie.net/callofduty/images/9/9b/Shipment_Load.jpg/revision/latest/scale-to-width-down/300?cb=20100723080524"
			},
			["af_chase"]                = new Map {Name = "Afghan Hunt"},
			["af_caves"]                = new Map {Name = "Afghan Caves"},
			["airport"]                 = new Map {Name = "Airport"},
			["arcadia"]                 = new Map {Name = "Arcadia"},
			["boneyard"]                = new Map {Name = "Airplane Scrapyard"},
			["so_crossing_so_bridge"]   = new Map {Name = "Bridge"},
			["cliffhanger"]             = new Map {Name = "Cliffhanger"},
			["co_hunted"]               = new Map {Name = "Hunted "},
			["so_forest_contingency"]   = new Map {Name = "Contingency"},
			["dc_whitehouse"]           = new Map {Name = "Whitehouse (Trumps Whitehouse in 2020)"},
			["dc_burning"]              = new Map {Name = "DC Burning"},
			["dc_emp"]                  = new Map {Name = "DC Dark"},
			["ending"]                  = new Map {Name = "Museum"},
			["estate"]                  = new Map {Name = "Makarov's House"},
			["favela"]                  = new Map {Name = "Roja's Favelas"},
			["so_showers_gulag"]        = new Map {Name = "Gulag"},
			["so_killspree_invasion"]   = new Map {Name = "BurgerTown"},
			["oilrig"]                  = new Map {Name = "Oilrig"},
			["roadkill"]                = new Map {Name = "School"},
			["so_hidden_so_ghillies"]   = new Map {Name = "Chernobyl"},
			["so_killspree_trainer"]    = new Map {Name = "The Pit"},
			["so_defuse_favela_escape"] = new Map {Name = "Favela Escape"}
		};

		public Dictionary<string, string> GameType { get; set; } = new Dictionary<string, string>
		{
			["war"]      = "Team Deathmatch",
			["dm"]       = "Free for all",
			["dom"]      = "Domination",
			["koth"]     = "Headquarters",
			["sab"]      = "Sabotage",
			["sd"]       = "Search and Destroy",
			["arena"]    = "Arena",
			["dd"]       = "Demolition",
			["ctf"]      = "Capture the Flag",
			["oneflag"]  = "One-Flag CTF",
			["gtnw"]     = "Global Thermo-Nuclear War",
			["infected"] = "Infected",
			["zombies"]  = "Zombies",
			["gg"]       = "Gungame",
			["gungame"]  = "Gungame",
			["dropzone"] = "DropZone"
		};

		public Dictionary<string, string> Color { get; set; } = new Dictionary<string, string>
		{
			["0"] = "Black",
			["1"] = "Red",
			["2"] = "Green",
			["3"] = "Yellow",
			["4"] = "Blue",
			["5"] = "Cyan",
			["6"] = "Pink",
			["7"] = "White",
			["8"] = "Brown",
			["9"] = "Gray",
			[":"] = "Rainbow"
		};

		public Dictionary<string, Color> DrawingColor { get; set; } = new Dictionary<string, Color>
		{
			["0"] = System.Drawing.Color.Black,
			["1"] = System.Drawing.Color.Red,
			["2"] = System.Drawing.Color.LawnGreen,
			["3"] = System.Drawing.Color.Yellow,
			["4"] = System.Drawing.Color.Blue,
			["5"] = System.Drawing.Color.Cyan,
			["6"] = System.Drawing.Color.HotPink,
			["7"] = System.Drawing.Color.White,
			["8"] = System.Drawing.Color.SaddleBrown,
			["9"] = System.Drawing.Color.Gray,
			[":"] = System.Drawing.Color.Empty
		};

		public CommonResources RestoreDefaults ( ) => this;

		public bool TryValidate ( out IList<Exception> exceptions )
		{
			exceptions = new List<Exception> ( );
			return exceptions.Count == 0;
		}
	}
}