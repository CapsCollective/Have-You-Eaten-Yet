using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Tray : MonoBehaviour
{
   private int _placedDumplings = 0;
   [SerializeField] private List<DropTarget> targets;
   
   private void Start()
   {
      DragAndDrop.OnPlaced += PlaceDumpling;
   }

   private void PlaceDumpling(DraggableType type)
   {
      if (type != DraggableType.Dumpling) return;
      WrapperThrower.SpawnedWrappers--;
      if (++_placedDumplings < 4) return;
      _placedDumplings = 0;
      transform.DOMove(new Vector3(110, -12, 0), 1f).OnComplete(() =>
      {
         foreach (DropTarget target in targets) Destroy(target.dropped);
         transform.DOMove(new Vector3(55, -12, 0), 1f).SetDelay(0.5f);
      }).SetDelay(1);
   } 
}
