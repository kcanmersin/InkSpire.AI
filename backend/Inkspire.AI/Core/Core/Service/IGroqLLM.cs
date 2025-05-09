﻿using Core.Data.Entity;
using Core.Data.Entity.Game;
using Core.Data.Entity.User;
using InkSpire.Core.Data.Entity;

namespace InkSpire.Application.Abstractions
{
    public interface IGroqLLM
    {
            Task<GameTopic> GenerateGameTopicAsync(string language);
            Task<GameAnswer> CheckAnswerAsync(string word, string topic, string language);

        Task<string> GenerateContentAsync(string title, string language, string level);
        Task<List<string>> GetImageIdeasAsync(string content);

        Task<Test> CheckTestAsync(Test test, string content);


        Task<Test> GenerateTestAsync(Book? book, AppUser? user, string level, string content, string language);

        //
    }
}
