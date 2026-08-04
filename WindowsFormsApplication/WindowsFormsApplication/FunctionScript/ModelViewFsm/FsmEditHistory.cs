using System.Collections.Generic;

namespace WindowsFormsApplication.ModelViewFsm
{
    /// <summary>基于完整模型快照的撤销/重做栈。</summary>
    public class FsmEditHistory
    {
        private readonly List<FsmModel> _undo = new();
        private readonly List<FsmModel> _redo = new();

        public int MaxDepth { get; set; } = 40;
        public bool CanUndo => _undo.Count > 0;
        public bool CanRedo => _redo.Count > 0;

        public void PushUndo(FsmModel snapshot)
        {
            if (snapshot is null) return;
            _undo.Add(snapshot);
            while (_undo.Count > MaxDepth)
                _undo.RemoveAt(0);
            _redo.Clear();
        }

        public bool TryUndo(FsmModel current, out FsmModel previous)
        {
            previous = null!;
            if (_undo.Count == 0) return false;
            previous = _undo[_undo.Count - 1];
            _undo.RemoveAt(_undo.Count - 1);
            _redo.Add(current);
            return true;
        }

        public bool TryRedo(FsmModel current, out FsmModel next)
        {
            next = null!;
            if (_redo.Count == 0) return false;
            next = _redo[_redo.Count - 1];
            _redo.RemoveAt(_redo.Count - 1);
            _undo.Add(current);
            return true;
        }

        public void Clear()
        {
            _undo.Clear();
            _redo.Clear();
        }
    }
}
