using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace Map.MapGenerator
{
    public class MapGenerator : MonoBehaviour
    {
        private IMapCreator _creator;
        private IMapFetcher _fetcher;
        private IMapSpawner _spawner;

        private int _iteration;
        public MapGenerator(int iteration)
        {
            this._iteration = iteration;
        }

        public MapGenerator SetCreator(IMapCreator creator)
        {
            this._creator = creator;
            return this;
        }
        public MapGenerator SetFetcher(IMapFetcher fetcher)
        {
            this._fetcher = fetcher;
            return this;
        }
        public MapGenerator SetSpawner(IMapSpawner spawner)
        {
            this._spawner = spawner;
            return this;
        }

        //IMapCreator : 맵의 레이아웃을 구성하는 단계
        //IMapFetcher : 맵의 레이아웃을 토대로 Map 오브젝트 구성하는 단계
        //IMapSpawner : 움직이는 Entity를 구성하는 단계

        public async void Generate(MapModel model)
        {
            MapLayout layout = null;
            model._isProgress = true;

            MapProbabilityTable table = DungeonManager.ins.table;
            await Task.Run(() =>
            {
                model._progressGenerateMap = 0.0f;

                layout = _creator.Create(
                    model.Size.x,
                    model.Size.y,
                    model.KeyCount,
                    table,
                    _iteration,
                    ref model._progressGenerateMap);

                model.StartPos = layout.StartPos;
                model.SolveRoute = layout.SolvingRoute;
            });
            MapBrain.ins.Process();
            Debug.Log("Map Generate OK");

            _fetcher.Fetch(model, layout);
            MapBrain.ins.Process();
            Debug.Log("Map Fetch OK");

            _spawner.SpawnPlayer(model);
            MapBrain.ins.Process();
            Debug.Log("Map Spawn Player OK");

            _spawner.Spawn(model);
            MapBrain.ins.Process();
            Debug.Log("Map Spawn OK");

            model._isProgress = false;
        }
    }
}
