﻿/*
    Copyright 2015 MCGalaxy
        
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
using MCGalaxy.Blocks;

namespace MCGalaxy.Network {

    public static partial class Packet {
        
        /// <summary> Constructs a handshake/motd packet. The text is shown to clients during map loading. </summary>
        /// <remarks> Some clients recognise special modifiers such as -hax +fly in this packet. </remarks>
        public static byte[] Motd(Player p, string motd) {
            byte[] buffer = new byte[131];
            buffer[0] = Opcode.Handshake;
            buffer[1] = Server.version;
            
            if (motd.Length > NetUtils.StringSize) {
                NetUtils.Write(motd, buffer, 2, p.hasCP437);
                NetUtils.Write(motd.Substring(NetUtils.StringSize), buffer, 66, p.hasCP437);
            } else {
                NetUtils.Write(Server.name, buffer, 2, p.hasCP437);
                NetUtils.Write(motd, buffer, 66, p.hasCP437);
            }

            buffer[130] = BlockPerms.CanModify(p, Block.blackrock) ? (byte)100 : (byte)0;
            return buffer;
        }
        
        /// <summary> Constructs a ping packet. </remarks>
        public static byte[] Ping() {
            return new byte[] { Opcode.Ping };
        }
        
        /// <summary> Constructs a packet that specified map data is about to be sent. </remarks>
        public static byte[] LevelInitalise() {
            return new byte[] { Opcode.LevelInitialise };
        }
        
        /// <summary> Constructs a packet describing the dimensions of a level. </summary>
        public static byte[] LevelFinalise(ushort width, ushort height, ushort length) {
            byte[] buffer = new byte[7];
            buffer[0] = Opcode.LevelFinalise;
            NetUtils.WriteU16(width, buffer, 1);
            NetUtils.WriteU16(height, buffer, 3);
            NetUtils.WriteU16(length, buffer, 5);
            return buffer;
        }
        
        /// <summary> Constructs a packet that adds/spawns an entity. </summary>
        public static byte[] AddEntity(byte id, string name, Position pos, 
                                       Orientation rot, bool hasCP437, bool extPos) {
            byte[] buffer = new byte[74 + (extPos ? 6 : 0)];
            buffer[0] = Opcode.AddEntity;
            buffer[1] = id;
            NetUtils.Write(name.RemoveLastPlus(), buffer, 2, hasCP437);
            
            int offset = NetUtils.WritePos(pos, buffer, 66, extPos);
            buffer[66 + offset] = rot.RotY;
            buffer[67 + offset] = rot.HeadX;
            return buffer;
        }
        
        /// <summary> Constructs an absolute position/teleport and rotation movement packet. </summary>
        public static byte[] Teleport(byte id, Position pos, Orientation rot, bool extPos) {
            byte[] buffer = new byte[10 + (extPos ? 6 : 0)];
            buffer[0] = Opcode.EntityTeleport;
            buffer[1] = id;
            
            int offset = NetUtils.WritePos(pos, buffer, 2, extPos);
            buffer[2 + offset] = rot.RotY;
            buffer[3 + offset] = rot.HeadX;
            return buffer;
        }

        /// <summary> Constructs a packet that removes/despawns an entity. </summary>        
        public static byte[] RemoveEntity(byte id) {
            return new byte[] { Opcode.RemoveEntity, id };
        }
        
        /// <summary> Constructs a chat message packet with an empty message. </summary>
        public static byte[] BlankMessage() { return Message("", 0, false); }
        
        /// <summary> Constructs a chat message packet. </summary>
        public static byte[] Message(string message, CpeMessageType type, bool hasCp437) {
            byte[] buffer = new byte[66];
            buffer[0] = Opcode.Message;
            buffer[1] = (byte)type;
            NetUtils.Write(message, buffer, 2, hasCp437);
            return buffer;
        }
        
        /// <summary> Constructs a kick / disconnect packet with the given reason / message. </summary>
        public static byte[] Kick(string message, bool cp437) {
            byte[] buffer = new byte[65];
            buffer[0] = Opcode.Kick;
            NetUtils.Write(message, buffer, 1, cp437);
            return buffer;
        }        
                
        /// <summary> Constructs a set user type/permission packet. </summary>
        /// <remarks> For certain clients, sets whether they are allowed to place bedrock, use ophax, place liquids. </remarks>
        public static byte[] UserType(Player p) {
            byte[] buffer = new byte[2];
            buffer[0] = Opcode.SetPermission;
            buffer[1] = BlockPerms.CanModify(p, Block.blackrock) ? (byte)100 : (byte)0;
            return buffer;
        }
    }
}
