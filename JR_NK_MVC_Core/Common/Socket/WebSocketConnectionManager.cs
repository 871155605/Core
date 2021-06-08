using JR_NK_MVC_Core.Common.Logger;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Common.Socket
{
    public class WebSocketConnectionManager
    {
        public static readonly ConcurrentDictionary<string, WebSocket> sockets = new();

        /// <summary>
        /// 保持WebSocket连接
        /// </summary>
        /// <param name="context"></param>
        /// <param name="_logger"></param>
        /// <returns></returns>
        public static async Task OnConnectedAsync(HttpContext context, ILoggerHelper _logger)
        {
            string socketId = context.Request.Query["socketId"].ToString();
            _logger.Info(typeof(WebSocketConnectionManager),$"接收到的SocketId:{socketId}");
            WebSocket socket = context.WebSockets.AcceptWebSocketAsync().Result;
            #region 关闭正处于活跃状态相同ID的WebSocket
            if (sockets.TryGetValue(socketId, out WebSocket oldSocket))
            {
                if (oldSocket.State == WebSocketState.Open)
                {
                    _logger.Info(typeof(WebSocketConnectionManager), $"开始关闭正处于活跃状态相同ID的WebSocket:{socketId}-{oldSocket.State}");
                    await oldSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    oldSocket.Dispose();
                    _logger.Info(typeof(WebSocketConnectionManager), $"关闭完成正处于活跃状态相同ID的WebSocket:{socketId}-{oldSocket.State}");
                };
            }
            #endregion
            #region 保持SOCKET连接并实时接收客户端消息
            var flagAdd = sockets.TryAdd(socketId, socket);
            _logger.Info(typeof(WebSocketConnectionManager), $"保持SOCKET连接并实时接收客户端消息:{socketId}-{flagAdd}");
            ArraySegment<byte> buffer = new(new byte[2048]);
            WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                string userMsg = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                _logger.Info(typeof(WebSocketConnectionManager), $"{socketId}收到客户端发送的消息:{userMsg}");
                buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(userMsg));
                //处理业务逻辑
                await socket.SendAsync(buffer, result.MessageType, result.EndOfMessage, CancellationToken.None);
                result = await socket.ReceiveAsync(buffer, CancellationToken.None);
            }
            #endregion
            #region 收到客户端关闭指令,正常关闭
            if (socket.State == WebSocketState.CloseReceived) {
                _logger.Info(typeof(WebSocketConnectionManager), $"开始正常关闭:{socketId}-{socket.State}");
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                socket.Dispose();
                _logger.Info(typeof(WebSocketConnectionManager), $"完成正常关闭:{socketId}-{socket.State}");
            }
            var endFlagRemove = sockets.TryRemove(socketId, out _);
            _logger.Info(typeof(WebSocketConnectionManager), $"从缓存中清除{socketId}-{endFlagRemove}");
            #endregion
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socketId"></param>
        /// <param name="data"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Task SendMessageAsync(string socketId, string data, CancellationToken token = default)
        {
            if (sockets.TryGetValue(socketId, out WebSocket socket))
            {
                return SendMessageAsync(socket,data,token);
            }
            else {
                throw new Exception($"SOCKET ID :{socketId} IS NOT FOUND");
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="socketId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<string> ReceiveMessageAsync(string socketId, CancellationToken token = default)
        {
            if (sockets.TryGetValue(socketId, out WebSocket socket))
            {
                return await ReceiveMessageAsync(socket,token);
            }
            else
            {
                throw new Exception($"SOCKET ID :{socketId} IS NOT FOUND");
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Task SendMessageAsync(WebSocket socket, string data, CancellationToken token = default)
        {
                var buffer = Encoding.UTF8.GetBytes(data);
                var segment = new ArraySegment<byte>(buffer);
                return socket.SendAsync(segment, WebSocketMessageType.Text, true, token);
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<string> ReceiveMessageAsync(WebSocket socket, CancellationToken token = default)
        {
            try
            {
                Console.WriteLine("ReceiveMessageAsync");
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[2048]);
                using var message = new MemoryStream();
                WebSocketReceiveResult result;
                do
                {
                    token.ThrowIfCancellationRequested();
                    result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                    message.Write(buffer.Array, buffer.Offset, result.Count);
                } while (!result.EndOfMessage);
                message.Seek(0, SeekOrigin.Begin);
                if (result.MessageType != WebSocketMessageType.Text) return null;
                using var reader = new StreamReader(message, Encoding.UTF8);
                return await reader.ReadToEndAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
