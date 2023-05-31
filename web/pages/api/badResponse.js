// pages/api/storeQuestion.js
import { MongoClient } from 'mongodb';

export default async function handler(req, res) {
  const question = req.body.question;
  const answer = req.body.answer;
  const date = new Date();
  const uri = process.env.DB_CONN_STRING;
  const client = new MongoClient(uri);

  try {
    // Connect to the MongoDB cluster
    await client.connect();

    await client
      .db('test')
      .collection('badResponse')
      .insertOne({ Question: question, Answer: answer, Date: date });

    res.status(200).json({ message: 'Question and Answer stored successfully' });
  } catch (e) {
    console.error(e);
    res.status(500).json({ message: 'An error occurred while storing the question' });
  } finally {
    await client.close();
  }
}
