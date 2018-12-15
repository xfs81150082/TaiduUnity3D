using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.PhotonEngine.Model
{
    public class EnemyPositionModel
    {
        public List<EnemyPositionProperty> list = new List<EnemyPositionProperty>();

    }

    public class EnemyPositionProperty
    {
        public string guid;
        public Vector3Obj position;
        public Vector3Obj eulerAngles;
    }
}
