# Etown_chatbot Web
### The Following Instructions can be used to implement your own chatbot
## Development

1. Clone the repo or download the ZIP

```
git clone [github https url]
```

2. Install packages

First run `npm install yarn -g` to install yarn globally (if you haven't already).

Then run:

```
yarn install
```

After installation, you should now see a `node_modules` folder.

3. Set up your `.env` file
  Your `.env` file should look like this:

```
OPENAI_API_KEY= "sk-XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
PINECONE_API_KEY= "eXXXXXXXXXXXXXXXXXXXXXXXXXX"
PINECONE_ENVIRONMENT= "us-XXXX-gcp-free"
PINECONE_INDEX_NAME= "Your name"
# Mongo_db connection
DB_CONN_STRING="mongodb+srv://name:PASSWORD@DBNAME.vcmoiuc.mongodb.net/"
DB_NAME="XXXXXXX"
QUESTIONS_COLLECTION_NAME="XXXXXXX"

```

- Visit [openai](https://help.openai.com/en/articles/4936850-where-do-i-find-my-secret-api-key) to retrieve API keys and insert into your `.env` file.
- Visit [pinecone](https://pinecone.io/) to create and retrieve your API keys, and also retrieve your environment and index name from the dashboard.

  (Make sure pinecone is created with the dimensions of 1536 and using cosine similarity)
  (MongoDB should be a cloud collection)

4. In the `config` folder, replace the `PINECONE_NAME_SPACE` with a `namespace` where you'd like to store your embeddings on Pinecone when you run `npm run ingest`. This namespace will later be used for queries and retrieval.

5. In `utils/makechain.ts` chain change the `QA_PROMPT` for your own usecase. Change `modelName` in `new OpenAI` to `gpt-4`, if you have access to `gpt-4` api. Please verify outside this repo that you have access to `gpt-4` api, otherwise the application will not work.

## Convert your PDF files to embeddings

**This repo can load multiple PDF files**

1. Inside `docs` folder, add your pdf files or folders that contain pdf files.

2. Run the script `npm run ingest` to 'ingest' and embed your docs. If you run into errors troubleshoot below.

3. Check Pinecone dashboard to verify your namespace and vectors have been added.

## Run the app

Once you've verified that the embeddings and content have been successfully added to your Pinecone, you can run the app `npm run dev` to launch the local dev environment, and then type a question in the chat interface.

## Troubleshooting

Sometimes the Pinecone index doesn't work initially. Just try deleting and remaking the index if you are having problems with the ingest.
Pinecone Indexes in the free tier are also removed after 7 days of inactivity so keep this in mind.

## Credit
Also check out [gpt4-pdf-chatbot-langchain](https://github.com/mayooear/gpt4-pdf-chatbot-langchain)

