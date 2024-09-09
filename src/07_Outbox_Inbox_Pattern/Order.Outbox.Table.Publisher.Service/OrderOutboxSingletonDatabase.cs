﻿using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Order.Outbox.Table.Publisher.Service;

public static class OrderOutboxSingletonDatabase
{
    private readonly static IDbConnection _connection;
    private static bool _dataReaderState = true; // ilk adımda db işlemleri başlasın

    static OrderOutboxSingletonDatabase()
    {
        _connection = new SqlConnection("...");
    }

    public static IDbConnection Connection
    {
        get
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }

            return _connection;
        }
    }

	public static bool DataReaderState => _dataReaderState;

	public static async Task<IEnumerable<T>> QueryAsync<T>(string sql) => await _connection.QueryAsync<T>(sql);

	public static async Task<int> ExecuteAsync(string sql) => await _connection.ExecuteAsync(sql);

	public static void DataReaderReady() => _dataReaderState = true;

	public static void DataReaderBusy() => _dataReaderState = false;
}
