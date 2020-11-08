using System;
using System.Collections.Generic;
using System.Linq;
using APIs;
using Firebase.Database;
using Serializables;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public GameInfo currentGameInfo;

        private Dictionary<string, bool> readyPlayers;
        private KeyValuePair<DatabaseReference, EventHandler<ChildChangedEventArgs>> readyListener;
        // private KeyValuePair<DatabaseReference, EventHandler<ValueChangedEventArgs>> localPlayerTurnListener;
        private KeyValuePair<DatabaseReference, EventHandler<ValueChangedEventArgs>> currentGameInfoListener;
        private KeyValuePair<DatabaseReference, EventHandler<ValueChangedEventArgs>> moveListener;

        // private readonly Dictionary<string, KeyValuePair<DatabaseReference, EventHandler<ChildChangedEventArgs>>>
        //     moveListeners =
        //         new Dictionary<string, KeyValuePair<DatabaseReference, EventHandler<ChildChangedEventArgs>>>();

        public void GetCurrentGameInfo(string gameId, string localPlayerId, Action<GameInfo> callback,
            Action<AggregateException> fallback)
        {
            currentGameInfoListener =
                DatabaseAPI.ListenForValueChanged($"games/{gameId}/gameInfo", args =>
                {
                    if (!args.Snapshot.Exists) return;

                    var gameInfo =
                        StringSerializationAPI.Deserialize(typeof(GameInfo), args.Snapshot.GetRawJsonValue()) as
                            GameInfo;
                    currentGameInfo = gameInfo;
                    currentGameInfo.localPlayerId = localPlayerId;
                    DatabaseAPI.StopListeningForValueChanged(currentGameInfoListener);
                    callback(currentGameInfo);
                }, fallback);
        }

        public void SetLocalPlayerReady(Action callback, Action<AggregateException> fallback)
        {
            DatabaseAPI.PostObject($"games/{currentGameInfo.gameId}/ready/{currentGameInfo.localPlayerId}", true,
                callback,
                fallback);
        }

        public void ListenForAllPlayersReady(IEnumerable<string> playersId, Action<string> onNewPlayerReady,
            Action onAllPlayersReady,
            Action<AggregateException> fallback)
        {
            readyPlayers = playersId.ToDictionary(playerId => playerId, playerId => false);
            readyListener = DatabaseAPI.ListenForChildAdded($"games/{currentGameInfo.gameId}/ready/", args =>
            {
                readyPlayers[args.Snapshot.Key] = true;
                onNewPlayerReady(args.Snapshot.Key);
                if (!readyPlayers.All(readyPlayer => readyPlayer.Value)) return;
                StopListeningForAllPlayersReady();
                onAllPlayersReady();
            }, fallback);
        }

        public void StopListeningForAllPlayersReady() => DatabaseAPI.StopListeningForChildAdded(readyListener);

        public string GetFirstPlayerId()
        {
            return currentGameInfo.playersIds[0];
        }

        public void ListenForMoves(Action<Move> onNewMove)
        {
            moveListener = DatabaseAPI.ListenForValueChanged(
                $"games/{currentGameInfo.gameId}/move",
                args =>
                {
                    if (!args.Snapshot.Exists) return;
                    var move = StringSerializationAPI.Deserialize(typeof(Move), args.Snapshot.GetRawJsonValue()) as Move;
                    if (move.userId == MainManager.Instance.currentUserId) return;
                    onNewMove(move);
                },
                exc => Debug.Log("!!!Fallback subscribe for move!!!"));
        }

        public void StopListenForMoves()
        {
            DatabaseAPI.StopListeningForValueChanged(moveListener);
        }

        public void SendMove(Move move)
        {
            DatabaseAPI.PostObject($"games/{currentGameInfo.gameId}/move",
                move,
                () => {},
                exc => Debug.Log("!!!Fallback send move!!!"));
        }

        // public void SendMove(Move move, Action callback, Action<AggregateException> fallback)
        // {
        //     DatabaseAPI.PushObject($"games/{currentGameInfo.gameId}/{currentGameInfo.localPlayerId}/moves/", move,
        //         () =>
        //         {
        //             Debug.Log("Moved sent successfully!");
        //             callback();
        //         }, fallback);
        // }

        // public void ListenForLocalPlayerTurn(Action onLocalPlayerTurn, Action<AggregateException> fallback)
        // {
        //     localPlayerTurnListener =
        //         DatabaseAPI.ListenForValueChanged($"games/{currentGameInfo.gameId}/turn", args =>
        //         {
        //             var turn =
        //                 StringSerializationAPI.Deserialize(typeof(string), args.Snapshot.GetRawJsonValue()) as string;
        //             if (turn == currentGameInfo.localPlayerId) onLocalPlayerTurn();
        //         }, fallback);
        // }

        // public void StopListeningForLocalPlayerTurn() =>
        //     DatabaseAPI.StopListeningForValueChanged(localPlayerTurnListener);

        // public void ListenForMoves(string playerId, Action<Move> onNewMove, Action<AggregateException> fallback)
        // {
        //     moveListeners.Add(playerId, DatabaseAPI.ListenForChildAdded(
        //         $"games/{currentGameInfo.gameId}/{playerId}/moves/",
        //         args => onNewMove(
        //             StringSerializationAPI.Deserialize(typeof(Move), args.Snapshot.GetRawJsonValue()) as Move),
        //         fallback));
        // }

        // public void StopListeningForMoves(string playerId)
        // {
        //     DatabaseAPI.StopListeningForChildAdded(moveListeners[playerId]);
        //     moveListeners.Remove(playerId);
        // }

        // public void SetTurnToOtherPlayer(string currentPlayerId, Action callback, Action<AggregateException> fallback)
        // {
        //     var otherPlayerId = currentGameInfo.playersIds.First(p => p != currentPlayerId);
        //     DatabaseAPI.PostObject(
        //         $"games/{currentGameInfo.gameId}/turn", otherPlayerId, callback, fallback);
        // }
    }
}