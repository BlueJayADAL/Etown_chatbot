import pickle
import re
import string
import docx2txt
import nltk

from nltk.corpus import stopwords
from nltk.tokenize import word_tokenize


def save_doc_as_txt(doc_loc, new_doc_loc):
    text = docx2txt.process(doc_loc)

    with open(new_doc_loc, 'wb') as text_file:
        print(text, file=text_file)


def read_doc(doc_loc):
    with open(doc_loc, 'rb') as text_file:
        return text_file.read()

    return None


def save_pkl_etownqa():
    doc_list = [line for line in str(dc.read_doc('data/EtownDocData.txt'), 'ISO-8859-1').split('\n') if line]

    combined_doc_list = [doc_list[i:i + 14] for i in range(0, len(doc_list), 14)]

    final_doc_list = []

    for group in range(len(combined_doc_list)):
        final_doc_list.append('')
        for element in combined_doc_list[group]:
            final_doc_list[group] += element

    with open('data/EtownQAData.pkl', 'wb') as f:
        pickle.dump(final_doc_list, f)


def clean_text(text):
    '''
    Make text lowercase,
    remove text in square brackets,
    remove punctuation and
    remove words containing numbers.
    '''
    text = text.lower()
    text = re.sub('\[.*?\]', '', text)
    text = re.sub('[%s]' % re.escape(string.punctuation), '', text)
    text = re.sub('\w*\d\w*', '', text)
    text = re.sub('[’‘’“”…]', '', text)
    text = re.sub('\n', '', text)
    return text


def remove_stop_words(text):
    nltk.download('stopwords')
    nltk.download('punkt')

    stop_words = list(stopwords.words('english'))

    # Add custom stop words to be eliminated
    stop_words.append('elizabethtown')
    stop_words.append('college')
    stop_words.append('etown')

    words = word_tokenize(text)
    text = ' '.join(str(j) for j in words if j not in stop_words and (len(j) != 1))

    return text


def encode_response(category):
    if isinstance(category, str) or category == -1:
        return 'Sorry, I did not understand that. Could you rephrase your question?'

    responses = ['Learn more about the history of Elizabethtown College at https://en.wikipedia.org/wiki/Elizabethtown_College',
                 'Check out student life at https://www.etown.edu/about',
                 'Find the location of Elizabethtown College at https://en.wikipedia.org/wiki/Elizabethtown_College',
                 'Check out the sports page at https://etownbluejays.com/',
                 'Tuition costs can be found at https://www.etown.edu/admissions/tuition-cost.aspx',
                 'Undergrad enrollment of 1,688 and the school lies on 203 acres',
                 'There is no greek life at Etown',
                 'A list of majors and minors can be found at https://www.etown.edu/academics/majors-minors.aspx',
                 'Classroom stats and info can be found at https://www.usnews.com/best-colleges/elizabethtown-college-3262',
                 'Professors and their information is located at https://www.etown.edu/directory',
                 'Important dates for Elizabethtown College can be found at https://www.etown.edu/offices/registration-records/academic-calendar-2022-23.aspx',
                 'Unique things about Elizabethtown is located at https://www.etown.edu/#:~:text=Why%20Etown%3F,the%20world%20needs%20more%20of.',
                 'Fun things to around Elizabethtown College can be found at https://www.tripadvisor.com/AttractionsNear-g52581-d5789493-Elizabethtown_College-Elizabethtown_Lancaster_County_Pennsylvania.html',
                 'More information on this can be found at https://www.usnews.com/best-colleges/elizabethtown-college-3262',
                 'Group work can be found at https://www.etown.edu/campus-life/student-clubs.aspx',
                 'Requirements by admissions can be looked at here: https://www.prepscholar.com/sat/s/colleges/Elizabethtown-College-admission-requirements#:~:text=Average%20GPA%3A%203.5&text=With%20a%20GPA%20of%203.5,like%20AP%20or%20IB%20classes.',
                 'Studying abroad information can be located at https://www.etown.edu/offices/study-abroad',
                 'Student life information can be found at https://www.etown.edu/campus-life/student-clubs.aspx',
                 'Dining services can be found at https://www.etown.edu/offices/dining/index.aspx',
                 'Information on living at Elizabethtown College can be found at https://www.etown.edu/offices/community-living/halls-apts/index.aspx',
                 'Wireless access at Elizabethtown College can be found at https://www.etown.edu/offices/its/Wireless_Access.aspx',
                 'Job rates after graduation can be found at https://www.etown.edu/admissions/outcomes.aspx#:~:text=96%25,within%20one%20year%20of%20graduation.',
                 'Commencement at Etown can be found at https://www.etown.edu/commencement',
                 'Accredidations at Elizabethtown College can be found at https://www.etown.edu/offices/institutional-research/accreditations.aspx',
                 'Etown has strong campus security, more info can be found at https://www.etown.edu/offices/security/index.aspx',
                 'Elizabethtown College statistics can be found at https://www.usnews.com/best-colleges/elizabethtown-college-3262#:~:text=Elizabethtown%20College%20has%20a%20total,of%20students%20live%20off%20campus.',
                 'Elizabethtown College clubs can be found at https://www.etown.edu/campus-life/student-clubs.aspx',
                 'The alumni association can be found at https://www.etownalumni.com/s/154/bp/home.aspx ']

    return responses[category]


def clean_speech(speech):
    to_replace = ['eternal', 'attendee down', 'always have a town college',
                  'he town', 'a town', 'each town', 'eat out',
                  'always a bit on college', 'eat em',
                  'always return college', 'eaten', 'eat home', 'town']

    for phrase in to_replace:
        speech = speech.replace(phrase, 'Etown')

    return speech


def is_junk(speech):
    junk_list = ['huh']

    for phrase in junk_list:
        if speech == phrase:
            return True

    return False
