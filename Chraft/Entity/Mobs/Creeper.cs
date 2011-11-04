#region C#raft License
// This file is part of C#raft. Copyright C#raft Team 
// 
// C#raft is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chraft.World;

namespace Chraft.Entity.Mobs
{
    public class Creeper : Monster
    {
        public override string Name
        {
            get { return "Creeper"; }
        }

        public override short MaxHealth { get { return 20; } }

        public override short AttackStrength
        {
            get
            {
                return 20; // 10 hearts (double when charged) varies based on proximity to blast radius
            }
        }

        internal Creeper(Chraft.World.WorldManager world, int entityId, Chraft.Net.MetaData data = null)
            : base(world, entityId, MobType.Creeper, data)
        {
        }

        protected override void DoDeath(EntityBase killedBy)
        {
            var killedByMob = killedBy as Mob;
            UniversalCoords coords = UniversalCoords.FromAbsWorld(Position.X, Position.Y, Position.Z);
            if (killedByMob != null && killedByMob.Type == MobType.Skeleton)
            {
                // If killed by a skeleton drop a music disc
                sbyte count = 1;
                short item;
                if (Server.Rand.Next(2) > 1)
                {
                    item = (short)BlockData.Items.Disc13;
                }
                else
                {
                    item = (short)BlockData.Items.DiscCat;
                }
                Server.DropItem(World, coords, new Interfaces.ItemStack(item, count, 0));
            }
            else
            {
                sbyte count = (sbyte)Server.Rand.Next(2);
                if (count > 0)
                    Server.DropItem(World, coords, new Interfaces.ItemStack((short)BlockData.Items.Gunpowder, count, 0));
            }
            base.DoDeath(killedBy);
        }
    }
}
