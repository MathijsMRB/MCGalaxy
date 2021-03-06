/*
    Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCGalaxy)
    
    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
    
    http://www.opensource.org/licenses/ecl2.php
    http://www.gnu.org/licenses/gpl-3.0.html
    
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
 */
using System;
using MCGalaxy.DB;

namespace MCGalaxy.Commands.Info {
    public sealed class CmdWhois : Command {
        public override string name { get { return "whois"; } }
        public override string shortcut { get { return "whowas"; } }
        public override string type { get { return CommandTypes.Information; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public override CommandPerm[] ExtraPerms {
            get { return new[] { new CommandPerm(LevelPermission.AdvBuilder, "+ can see player's IP and if on whitelist") }; }
        }
        public override CommandAlias[] Aliases {
            get { return new[] { new CommandAlias("info"), new CommandAlias("i") }; }
        }
        
        public override void Use(Player p, string message) {
            if (message == "") message = p.name;
            int matches;
            Player who = PlayerInfo.FindMatches(p, message, out matches);
            if (matches > 1) return;
            
            if (matches == 0) {
                if (!Formatter.ValidName(p, message, "player")) return;
                Player.Message(p, "Searching database for the player..");
                PlayerData target = PlayerInfo.FindOfflineMatches(p, message);
                if (target == null) return;
                
                foreach (OfflineStatPrinter printer in OfflineStat.Stats) {
                    printer(p, target);
                }
            } else {
                foreach (OnlineStatPrinter printer in OnlineStat.Stats) {
                    printer(p, who);
                }
            }
        }

        public override void Help(Player p) {
            Player.Message(p, "%T/whois [name]");
            Player.Message(p, "%HDisplays information about that player.");
            Player.Message(p, "%HNote: Works for both online and offline players.");
        }
    }
}