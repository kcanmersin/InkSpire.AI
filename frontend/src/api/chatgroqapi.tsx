

const groqApiKey = import.meta.env.VITE_GROQ_API_KEY || "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"; 

const groqApiRequest = async (messages: { role: string; content: string }[]) => {
  try {
    const response = await fetch("https://api.groq.com/openai/v1/chat/completions", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${groqApiKey}`,
      },
      body: JSON.stringify({
        model: "llama-3.3-70b-versatile",
        messages,
      }),
    });

    if (!response.ok) {
      throw new Error(`HTTP Error: ${response.status}`);
    }

    const data = await response.json();
    return data.choices?.[0]?.message?.content || null;
  } catch (error) {
    console.error("Groq API Error:", error);
    return null;
  }
};
export const translateText = async (text: string, targetLanguage: string): Promise<string> => {
  const prompt = `You are a professional translator. Please translate the following text into ${targetLanguage}. Respond only in JSON format with the key "translation":

{
  "translation": "Your translation here"
}

Text: "${text}"`;

  const response = await groqApiRequest([{ role: "user", content: prompt }]);
  try {
    return response ? JSON.parse(response).translation : "Çeviri alınamadı.";
  } catch (error) {
    console.error("JSON Parse Error:", error);
    return "Çeviri sırasında bir hata oluştu.";
  }
};

export const generateExamplesWithExplanations = async (
  text: string,
  bookLanguage: string
): Promise<{ sentence: string; explanation: string }[]> => {
  const prompt = `Generate 3 example sentences using the following phrase in natural language (${bookLanguage}). 
For each sentence, provide a short explanation of how the word or phrase is used, also in ${bookLanguage}.
Respond only in JSON format with the key "examples", as an array of objects, where each object contains "sentence" and "explanation":

{
  "examples": [
    {
      "sentence": "Example sentence 1 in ${bookLanguage}",
      "explanation": "Explanation for example sentence 1 in ${bookLanguage}"
    },
    {
      "sentence": "Example sentence 2 in ${bookLanguage}",
      "explanation": "Explanation for example sentence 2 in ${bookLanguage}"
    },
    {
      "sentence": "Example sentence 3 in ${bookLanguage}",
      "explanation": "Explanation for example sentence 3 in ${bookLanguage}"
    }
  ]
}

Phrase: "${text}"`;

  const response = await groqApiRequest([{ role: "user", content: prompt }]);
  try {
    return response ? JSON.parse(response).examples : [];
  } catch (error) {
    console.error("JSON Parse Error:", error);
    return [{ sentence: "Örnek alınamadı.", explanation: "Açıklama alınamadı." }];
  }
};

