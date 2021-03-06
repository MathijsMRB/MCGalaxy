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
using MCGalaxy.Commands.Building;
using MCGalaxy.Drawing.Ops;

namespace MCGalaxy.Drawing.Brushes {    
    public sealed class SolidBrush : Brush {
        readonly ExtBlock block;
        
        public SolidBrush(ExtBlock block) {
            this.block = block;
        }
        
        public override string Name { get { return "Normal"; } }
        
        public override ExtBlock NextBlock(DrawOp op) { return block; }
    }
    
    public sealed class StripedBrush : Brush {
        readonly ExtBlock b1, b2;
        
        public StripedBrush(ExtBlock block1, ExtBlock block2) {
           b1 = block1; b2 = block2;
        }
        
        public override string Name { get { return "Striped"; } }
        
        public override ExtBlock NextBlock(DrawOp op) {
            return ((op.Coords.X + op.Coords.Y + op.Coords.Z) & 3) <= 1 ? b1 : b2;
        }
    }
    
    public sealed class CheckeredBrush : Brush {
        readonly ExtBlock b1, b2;
        
        public CheckeredBrush(ExtBlock block1, ExtBlock block2) {
            b1 = block1; b2 = block2;
        }
        
        public override string Name { get { return "Checkered"; } }
        
        public override ExtBlock NextBlock(DrawOp op) {
            return ((op.Coords.X + op.Coords.Y + op.Coords.Z) & 1) == 0 ? b1 : b2;
        }
    }
}
