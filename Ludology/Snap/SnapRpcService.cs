namespace Ludology.Snap
{
    using System.Collections.Generic;
    using AustinHarris.JsonRpc;

    class SnapRpcService : GameRpcService
    {
        public override string GameDescription
        {
            get
            {
                return "Snap description";
            }
        }

        public override string GameName
        {
            get
            {
                return "SNAP";
            }
        }

        [JsonRpcMethod("Snap.NextMove")]
        public Dictionary<string, bool> OnNextMove(int gameId, string[] cards)
        {
            bool move;
            if (cards.Length < 2)
            {
                move = false;
            }
            else
            {
                string topCard = cards[cards.Length - 1];
                string beneathCard = cards[cards.Length - 2];
                move = topCard.Substring(0, topCard.Length - 1) == beneathCard.Substring(0, beneathCard.Length - 1);
            }

            return new Dictionary<string, bool>() { { "Snap", false } };
        }
    }
}
