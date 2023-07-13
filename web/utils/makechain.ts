import { OpenAI } from 'langchain/llms/openai';
import { PineconeStore } from 'langchain/vectorstores/pinecone';
import { ConversationalRetrievalQAChain } from 'langchain/chains';

const CONDENSE_PROMPT = `Given the following conversation and a follow up question, rephrase the follow up question to be a standalone question.

Chat History:
{chat_history}
Follow Up Input: {question}
Standalone question:`;

// const QA_PROMPT = '';
const QA_PROMPT = `You are a helpful AI assistant for Elizabethtown College also referred to as Etown.
If you don't know the answer, just say you don't know. DO NOT try to make up an answer.
If the question is not related to Elizabethtown, politely respond that you are tuned to only answer questions that are related to Elizabethtown.

{context}

Question: {question}
Helpful answer in markdown:`;

export const makeChain = (vectorstore: PineconeStore) => {
  const model = new OpenAI({
    temperature: .1, // increase temepreature to get more creative answers
    // modelName: 'gpt-3.5-turbo', //change this to gpt-4 if you have access
    modelName: 'gpt-4',
  });

  const chain = ConversationalRetrievalQAChain.fromLLM(
    model,
    vectorstore.asRetriever(),
    {
      qaTemplate: QA_PROMPT,
      questionGeneratorTemplate: CONDENSE_PROMPT,
    },
  );
  return chain;
};
