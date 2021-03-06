/*
    Copyright 2011 MCForge
    
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
using MCGalaxy.Events;

namespace MCGalaxy.Commands.Moderation {
    public sealed class CmdFreeze : Command {
        public override string name { get { return "freeze"; } }
        public override string shortcut { get { return "fz"; } }
        public override string type { get { return CommandTypes.Moderation; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

        public override void Use(Player p, string message) {
            if (message == "") { Help(p); return; }
            Player who = PlayerInfo.FindMatches(p, message);
            if (who == null) return;
            
            if (p == who) { Player.Message(p, "Cannot freeze yourself."); return; }
            if (p != null && who.Rank >= p.Rank) { 
                MessageTooHighRank(p, "freeze", false); return; 
            }
            
            ModActionType actionType = who.frozen ? ModActionType.Unfrozen : ModActionType.Frozen;
            ModAction action = new ModAction(who.name, p, actionType);
            OnModActionEvent.Call(action);
        }
        
        public override void Help(Player p) {
            Player.Message(p, "%T/freeze [name]");
            Player.Message(p, "%HStops [name] from moving until unfrozen.");
        }
    }
}
